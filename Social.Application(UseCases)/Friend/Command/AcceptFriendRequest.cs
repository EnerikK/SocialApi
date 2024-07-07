using MediatR;
using Social.Application_UseCases_.Models;

namespace Social.Application_UseCases_.FriendRequest.Command;

public class AcceptFriendRequest : IRequest<OperationResult<Unit>>
{
    public Guid FriendRequestId { get; set; }
    public Guid ActionPerformedById { get; set; }
}