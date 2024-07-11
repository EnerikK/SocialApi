using MediatR;
using Social.Application_UseCases_.Models;

namespace Social.Application_UseCases_.Friend.Command;

public class RejectFriendRequest : IRequest<OperationResult<Unit>>
{
    public Guid FriendRequestId { get; set; }
    public Guid ActionPerformedById { get; set; }
}