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

namespace Social.Application_UseCases_.Identity.Handlers;

public class RegisterIdentifyHandler : IRequestHandler<RegisterIdentify,OperationResult<string>>
{
    private readonly DataContext _dataContext;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IdentityService _identityService;
    
    public RegisterIdentifyHandler(DataContext dataContext, UserManager<IdentityUser> userManager,
        IOptions<IdentityService> identityService)
    {
        _dataContext = dataContext;
        _userManager = userManager;
        _identityService = identityService.Value;
    }

    public async Task<OperationResult<string>> Handle(RegisterIdentify request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<string>();

        try
        {
            var creationValidated = await ValidateIdentityDoesNotExist(result, request);
            if (!creationValidated) return result;

            await using var transaction = _dataContext.Database.BeginTransaction();
            
            var identity = await CreateIdentityUserAsync(result, request, transaction);
            if (identity == null) return result;

            var profile = await CreateUserProfileAsync(result, request, transaction, identity);
            await transaction.CommitAsync();

            var claimsIdentity = new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, identity.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, identity.Email),
                new Claim("IdentityId", identity.Id),
                new Claim("UserProfileId", profile.UserProfileId.ToString())
            });
            
            var token = _identityService.CreateSecurityToken(claimsIdentity);
            result.PayLoad = _identityService.WriteToken(token);
            
        }
        catch (UserProfileNotValidException ex)
        {
            result.IsError = true;
            ex.ValidationErrors.ForEach(valError =>
            {
                var error = new Error
                {
                    Code = ErrorCode.ValidationError,
                    Message = $"{ex.Message}"
                };
                result.IsError = true;
                result.Errors.Add(error);
            });
        }
        catch (Exception e)
        {
            var error = new Error
            {
                Code = ErrorCode.UnknownError,
                Message = $"{e.Message}"
            };
            result.IsError = true;
            result.Errors.Add(error);
        }

        return result;
    }
     private async Task<bool> ValidateIdentityDoesNotExist(OperationResult<string> result,
         RegisterIdentify request)
    {
        var existingIdentity = await _userManager.FindByEmailAsync(request.Username);

        if (existingIdentity != null)
        {
            result.IsError = true;
            var error = new Error { Code = ErrorCode.IdentityUserAlreadyExists, 
                Message = $"Provided email address already exists. Cannot register new user"};
            result.Errors.Add(error);
            return false;
        }

        return true;
    }

    private async Task<IdentityUser> CreateIdentityUserAsync(OperationResult<string> result,
        RegisterIdentify request, IDbContextTransaction transaction)
    {
        var identity = new IdentityUser {Email = request.Username, UserName = request.Username};
        var createdIdentity = await _userManager.CreateAsync(identity, request.Password);
        if (!createdIdentity.Succeeded)
        {
            await transaction.RollbackAsync();
            result.IsError = true;

            foreach (var identityError in createdIdentity.Errors)
            {
                var error = new Error { Code = ErrorCode.IdentityCreationFailed, 
                    Message = identityError.Description};
                result.Errors.Add(error);
            }
            return null;
        }

        return identity;
    }

    private async Task<UserProfile> CreateUserProfileAsync(OperationResult<string> result,
        RegisterIdentify request, IDbContextTransaction transaction, IdentityUser identity)
    {
        try
        {
            var profileInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.Username,
                request.Phone, request.DateOfBirth, request.CurrentCity);

            var profile = UserProfile.CreateUserProfile(identity.Id, profileInfo);
            _dataContext.UserProfiles.Add(profile);
            await _dataContext.SaveChangesAsync();
            return profile;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}