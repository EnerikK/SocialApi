using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Social.Application_UseCases_.Enums;
using Social.Application_UseCases_.Identity.Commands;
using Social.Application_UseCases_.Models;
using Social.Application_UseCases_.Options;
using Social.Application_UseCases_.Services;
using Social.DataAccess;
using Social.Domain.Aggregates.UserProfileAggregate;
using Social.Domain.Exceptions;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace Social.Application_UseCases_.Identity.Handlers;

public class RegisterIdentifyHandler : IRequestHandler<RegisterIdentify,OperationResult<string>>
{
    private readonly DataContext _dataContext;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IdentityService _identityService;
    
    public RegisterIdentifyHandler(DataContext dataContext, UserManager<IdentityUser> userManager,
        IdentityService identityService)
    {
        _dataContext = dataContext;
        _userManager = userManager;
        _identityService = identityService;
    }

    public async Task<OperationResult<string>> Handle(RegisterIdentify request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<string>();

        try
        {
            await ValidateIdentityDoesNotExist(result, request);
            if (result.IsError) return result;

            await using var transaction = await _dataContext.Database.BeginTransactionAsync(cancellationToken);
            
            var identity = await CreateIdentityUserAsync(result, request, transaction,cancellationToken);
            if (result.IsError) return result;

            var profile = await CreateUserProfileAsync(result, request, transaction, identity,cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            result.PayLoad = GetJWTString(identity, profile);
            return result;
        }
        catch (UserProfileNotValidException ex)
        {
            ex.ValidationErrors.ForEach(error => result.AddError(ErrorCode.ValidationError, error));
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }
        return result;
    }
     private async Task ValidateIdentityDoesNotExist(OperationResult<string> result,
         RegisterIdentify request)
    {
        var existingIdentity = await _userManager.FindByEmailAsync(request.Username);

        if(existingIdentity != null) result.AddError(ErrorCode.IdentityUserAlreadyExists,ErrorMessages.UserAlreadyExists);
    }

    private async Task<IdentityUser> CreateIdentityUserAsync(OperationResult<string> result,
        RegisterIdentify request, IDbContextTransaction transaction,CancellationToken cancellationToken)
    {
        var identity = new IdentityUser {Email = request.Username, UserName = request.Username};
        var createdIdentity = await _userManager.CreateAsync(identity, request.Password);
        if (!createdIdentity.Succeeded)
        {
            await transaction.RollbackAsync(cancellationToken);

            foreach (var identityError in createdIdentity.Errors)
            {
                result.AddError(ErrorCode.IdentityCreationFailed,identityError.Description);
            }
        }
        return identity;
    }
    private async Task<UserProfile> CreateUserProfileAsync(OperationResult<string> result,
        RegisterIdentify request, IDbContextTransaction transaction, IdentityUser identity,CancellationToken cancellationToken)
    {
        try
        {
            var profileInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.Username,
                request.Phone, request.DateOfBirth, request.CurrentCity);

            var profile = UserProfile.CreateUserProfile(identity.Id, profileInfo);
            _dataContext.UserProfiles.Add(profile);
            await _dataContext.SaveChangesAsync(cancellationToken);
            return profile;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
    
    //Small refactoring
    private string GetJWTString(IdentityUser identityUser, UserProfile userProfile)
    {
        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, identityUser.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, identityUser.Email),
            new Claim("IdentityId", identityUser.Id),
            new Claim("UserProfileId", userProfile.UserProfileId.ToString())
        });
     
        var token = _identityService.CreateSecurityToken(claimsIdentity);
        return _identityService.WriteToken(token);
    }
}