using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social.Api.Contracts.FriendRequest.Requests;
using Social.Api.Extensions;
using Social.Api.Filters;
using AutoMapper;
using MediatR;
using Social.Application_UseCases_.Friend.Command;
using Social.Application_UseCases_.FriendRequest.Command;

namespace Social.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class FriendRequestController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    
    public FriendRequestController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    [HttpPost]
    [Route(ApiRoutes.FriendRequest.FriendRequestCreate)]
    [ValidateModel]
    public async Task<IActionResult> SendFriendRequest(FriendRequestCreate friendRequestCreate,
        CancellationToken token)
    {
        var command = new CreateFriendRequest
        {
            RequesterId = friendRequestCreate.RequesterId,
            ReiceiverId = friendRequestCreate.ReceiverId
        };
        var result = await _mediator.Send(command, token);
        if (result.IsError) HandleErrorResponse(result.Errors);
        return NoContent();
    }

    [HttpPost]
    [Route(ApiRoutes.FriendRequest.FriendRequestAccept)]
    [ValidateGuid("friendRequestId")]
    public async Task<IActionResult> AcceptFriendRequest(Guid friendRequestId, CancellationToken token)
    {
        var actionPerformedBy = HttpContext.GetUserProfileClaimValue();
        var command = new AcceptFriendRequest
        {
            FriendRequestId = friendRequestId,
            ActionPerformedById = actionPerformedBy
        };
        var result = await _mediator.Send(command,token);
        if (result.IsError) return HandleErrorResponse(result.Errors);
        return NoContent();
    }

    [HttpPost]
    [Route(ApiRoutes.FriendRequest.FriendRequestReject)]
    [ValidateGuid("friendRequestId")]
    public async Task<IActionResult> RejectFriendRequest(Guid friendRequestId, CancellationToken token)
    {
        var actionPerformedBy = HttpContext.GetUserProfileClaimValue();
        var command = new RejectFriendRequest
        {
            FriendRequestId = friendRequestId,
            ActionPerformedById = actionPerformedBy
        };
        var result = await _mediator.Send(command,token);
        if (result.IsError) return HandleErrorResponse(result.Errors);
        return NoContent();
    }
}