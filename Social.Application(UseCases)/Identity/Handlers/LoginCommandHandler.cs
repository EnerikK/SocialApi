using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Social.Application_UseCases_.Enums;
using Social.Application_UseCases_.Identity.Commands;
using Social.Application_UseCases_.Models;
using Social.Application_UseCases_.Services;
using Social.DataAccess;
using Social.Domain.Aggregates.UserProfileAggregate;

namespace Social.Application_UseCases_.Identity.Handlers;

public class LoginCommandHandler : IRequestHandler<LoginCommand,OperationResult<string>>
{
    private readonly DataContext _dataContext;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IdentityService _identityService;

    public LoginCommandHandler(DataContext dataContext , UserManager<IdentityUser> userManager,IdentityService identityService)
    {
        _dataContext = dataContext;
        _userManager = userManager;
        _identityService = identityService;
    }
    
    public async Task<OperationResult<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<string>();
        
        try
        {
            var identityUser = await ValidateAndGetIdentity(request, result);
            if (result.IsError) return result;
            
            var userProfile =
                await _dataContext.UserProfiles.FirstOrDefaultAsync(userP => userP.IdentityId == identityUser.Id,cancellationToken);
            result.PayLoad = GetJWTString(identityUser, userProfile);
            return result;
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }

        return result;
    }
    private async Task<IdentityUser> ValidateAndGetIdentity(LoginCommand request, OperationResult<string> result)
    {
        
        var identityUser = await _userManager.FindByEmailAsync(request.Username);
        if (identityUser is null) result.AddError(ErrorCode.IdentityUserAlreadyExists,ErrorMessages.NoExistingUser);

        var validPassword = await _userManager.CheckPasswordAsync(identityUser, request.Password);
        if(!validPassword) result.AddError(ErrorCode.IdentityUserDoesNotExist,ErrorMessages.IncorrectPassword);

        return identityUser;
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