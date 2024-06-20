using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Social.Api.Contracts.Identity;
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
    public async Task<IActionResult> Register(UserRegistration registration)
    {
        var command = _mapper.Map<RegisterIdentify>(registration);
        var result = await _mediator.Send(command);

        if (result.IsError) return HandleErrorResponse(result.Errors);

        var authenticationResult = new AuthenticationResult() { Token = result.PayLoad };

        return Ok(authenticationResult);
    }

    [HttpPost]
    [Route(ApiRoutes.Identity.Login)]
    [ValidateModel]
    public async Task<IActionResult> Login(Login login)
    {
        var command = _mapper.Map<LoginCommand>(login);
        var result = await _mediator.Send(command);
        
        if (result.IsError) return HandleErrorResponse(result.Errors);

        var authenticationResult = new AuthenticationResult() { Token = result.PayLoad };
        return Ok(authenticationResult);
    }
}