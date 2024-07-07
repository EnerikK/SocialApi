using MediatR;
using Social.Application_UseCases_.Models;

namespace Social.Application_UseCases_.FriendRequest.Command;

public class CreateFriendRequest : IRequest<OperationResult<Unit>>
{
    public Guid RequesterId { get; set; }
    public Guid ReiceiverId { get; set; }
}