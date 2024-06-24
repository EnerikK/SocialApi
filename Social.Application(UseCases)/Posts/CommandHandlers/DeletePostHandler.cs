using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application_UseCases_.Enums;
using Social.Application_UseCases_.Models;
using Social.Application_UseCases_.Posts.Commands;
using Social.DataAccess;
using Social.Domain.Aggregates.PostAggregate;

namespace Social.Application_UseCases_.Posts.CommandHandlers;

public class DeletePostHandler : IRequestHandler<DeletePost,OperationResult<Post>>
{
    public readonly DataContext _dataContext;

    public DeletePostHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<OperationResult<Post>> Handle(DeletePost request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Post>();
        try
        {
            var post = await _dataContext.Posts.FirstOrDefaultAsync(post => post.PostId == request.PostId,cancellationToken: cancellationToken);
            if (post is null)
            {
                result.AddError(ErrorCode.NotFound,string.Format(PostErrorMessages.PostNotFound,request.PostId));
            }

            if (post.UserProfileId != request.UserProfileId)
            {
                result.AddError(ErrorCode.DeletePostNotPossible,PostErrorMessages.PostDeleteNotPossible);
                return result;
            }

            _dataContext.Posts.Remove(post);
            await _dataContext.SaveChangesAsync(cancellationToken);
            result.PayLoad = post;
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }

        return result;
    }
}