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
            var identityUser = await _userManager.FindByEmailAsync(request.Username);
            if (identityUser is null)
            {
                result.IsError = true;
                var error = new Error
                {
                    Code = ErrorCode.IdentityUserDoesNotExist,
                    Message = $"Unable to find the user , check the username"
                };
                result.Errors.Add(error);
                return result;
            }

            var validPassword = await _userManager.CheckPasswordAsync(identityUser, request.Password);
            if (!validPassword)
            {
                var error = new Error
                {
                    Code = ErrorCode.IncorrectPassword,
                    Message = $"Wrong Password you dummy , please try again :3"
                };
                result.IsError = true;
                result.Errors.Add(error);
                return result;
            }

            var userProfile =
                await _dataContext.UserProfiles.FirstOrDefaultAsync(userP => userP.IdentityId == identityUser.Id,cancellationToken);
            result.PayLoad = GetJWTString(identityUser, userProfile);
            return result;
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