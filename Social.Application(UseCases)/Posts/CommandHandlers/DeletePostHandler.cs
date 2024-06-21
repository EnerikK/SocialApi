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
                result.IsError = true;
                var error = new Error
                {
                    Code = ErrorCode.NotFound,
                    Message = $"No Post Found With ID {request.PostId}"
                };
                result.Errors.Add(error);
                return result;
            }

            if (post.UserProfileId != request.UserProfileId)
            {
                result.IsError = true;
                var error = new Error
                {
                    Code = ErrorCode.DeletePostNotPossible,
                    Message = $"Only the one that posted the post can delete the post"
                };
                result.Errors.Add(error);
                return result;
            }

            _dataContext.Posts.Remove(post);
            await _dataContext.SaveChangesAsync(cancellationToken);
            result.PayLoad = post;
        }
        catch (Exception e)
        {
            var error = new Error
            {
                Code = ErrorCode.UnknownError,
                Message = $"{e.Message}"
            };
            result.IsError = true;
            result.Errors.Add(error);
        }

        return result;
    }
}