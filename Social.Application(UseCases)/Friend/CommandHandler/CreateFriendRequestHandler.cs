using MediatR;
using Social.Application_UseCases_.Enums;
using Social.Application_UseCases_.FriendRequest.Command;
using Social.Application_UseCases_.Models;
using Social.DataAccess;
using Social.Domain.Exceptions;

namespace Social.Application_UseCases_.Friend.CommandHandler;

public class CreateFriendRequestHandler : IRequestHandler<CreateFriendRequest,OperationResult<Unit>>
{
    private readonly DataContext _dataContext;

    public CreateFriendRequestHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<OperationResult<Unit>> Handle(CreateFriendRequest request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Unit>();

        try
        {
            var friendRequest = Domain.Aggregates.FriendRequests.FriendRequest.CreateFriendRequest(Guid.NewGuid(),
                request.RequesterId, request.ReiceiverId, DateTime.UtcNow);
            _dataContext.FriendRequests.Add(friendRequest);
        }
        catch (FriendRequestValidationException ex)
        {
            result.AddError(ErrorCode.FriendRequestValidationError, ex.Message);
        }
        catch (Exception e)
        {
            result.AddError(ErrorCode.DatabaseOperationException, e.Message);
        }

        return result;
    }
}