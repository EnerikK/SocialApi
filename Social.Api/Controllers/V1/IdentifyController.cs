using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social.Api.Contracts.Identity;
using Social.Api.Extensions;
using Social.Api.Filters;
using Social.Application_UseCases_.Identity.Commands;

namespace Social.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
public class IdentifyController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public IdentifyController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    [Route(ApiRoutes.Identity.Registration)]
    [ValidateModel]
    public async Task<IActionResult> Register(UserRegistration registration,CancellationToken cancellationToken)
    {
        var command = _mapper.Map<RegisterIdentify>(registration);
        var result = await _mediator.Send(command ,cancellationToken);

        if (result.IsError) return HandleErrorResponse(result.Errors);

        var authenticationResult = new AuthenticationResult() { Token = result.PayLoad };

        return Ok(authenticationResult);
    }

    [HttpPost]
    [Route(ApiRoutes.Identity.Login)]
    [ValidateModel]
    public async Task<IActionResult> Login(Login login, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<LoginCommand>(login);
        var result = await _mediator.Send(command , cancellationToken);
        
        if (result.IsError) return HandleErrorResponse(result.Errors);

        var authenticationResult = new AuthenticationResult() { Token = result.PayLoad };
        return Ok(authenticationResult);
    }

    [HttpDelete]
    [Route(ApiRoutes.Identity.IdentityById)]
    [ValidateGuid("identityUserId")]
    [Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteAccount(string identityUserId, CancellationToken token)
    {
        var identityUserGuid = Guid.Parse(identityUserId);
        var requestedGuid = HttpContext.GetIdentityIdClaimValue();

        var command = new RemoveAccount
        {
            IdentityUserId = identityUserGuid,
            RequestedGuid = requestedGuid
        };

        var result = await _mediator.Send(command,token);
        if (result.IsError) HandleErrorResponse(result.Errors);
        return NoContent();
    }
}