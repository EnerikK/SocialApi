using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application_UseCases_.Enums;
using Social.Application_UseCases_.FriendRequest.Command;
using Social.Application_UseCases_.Models;
using Social.DataAccess;

namespace Social.Application_UseCases_.FriendRequest.CommandHandler;

public class AcceptFriendRequestHandler : IRequestHandler<AcceptFriendRequest,OperationResult<Unit>>
{
    private readonly DataContext _dataContext;

    public AcceptFriendRequestHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<OperationResult<Unit>> Handle(AcceptFriendRequest request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Unit>();

        var friendRequest = await _dataContext.FriendRequests.FirstOrDefaultAsync(
            friendRequest => friendRequest.FriendRequestId == request.FriendRequestId &&
                             friendRequest.ReceiverUserProfileId == request.ActionPerformedById, cancellationToken);

        if (friendRequest is null)
        {
            result.AddError(ErrorCode.FriendRequestAcceptNotPossible, $"not possible to accept this friend request");
        }

        var friend = friendRequest.AcceptFriendRequest(Guid.NewGuid());
        
        await using var transaction = await  _dataContext.Database.BeginTransactionAsync(cancellationToken);
        _dataContext.FriendRequests.Update(friendRequest);
        _dataContext.FriendStatus.Add(friend!);

        try
        {
            await _dataContext.SaveChangesAsync(cancellationToken);
            await _dataContext.Database.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await _dataContext.Database.RollbackTransactionAsync(cancellationToken);
            result.AddError(ErrorCode.DatabaseOperationException,e.Message);
        }

        return result;
    }
}