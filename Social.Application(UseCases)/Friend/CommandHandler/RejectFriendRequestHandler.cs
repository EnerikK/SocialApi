using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application_UseCases_.Enums;
using Social.Application_UseCases_.Friend.Command;
using Social.Application_UseCases_.FriendRequest.Command;
using Social.Application_UseCases_.Models;
using Social.DataAccess;

namespace Social.Application_UseCases_.Friend.CommandHandler;

public class RejectFriendRequestHandler : IRequestHandler<RejectFriendRequest,OperationResult<Unit>>
{
    private readonly DataContext _dataContext;

    public RejectFriendRequestHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public async Task<OperationResult<Unit>> Handle(RejectFriendRequest request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Unit>();

        var friendRequest = await _dataContext.FriendRequests.FirstOrDefaultAsync(
            friendRequest => friendRequest.FriendRequestId == request.FriendRequestId &&
                             friendRequest.ReceiverUserProfileId == request.ActionPerformedById, cancellationToken);

        if (friendRequest == null)
        {
            result.AddError(ErrorCode.FriendRequestRejectNotPossible,"rejection of this friend request is not possible");
            return result;
        }
        
        friendRequest.RejectFriendRequest();
        _dataContext.FriendRequests.Update(friendRequest);

        try
        {
            await _dataContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            result.AddError(ErrorCode.DatabaseOperationException, e.Message);
        }

        return result;
    }
}