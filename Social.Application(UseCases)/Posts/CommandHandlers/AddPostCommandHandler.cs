using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application_UseCases_.Enums;
using Social.Application_UseCases_.Models;
using Social.Application_UseCases_.Posts.Commands;
using Social.DataAccess;
using Social.Domain.Aggregates.PostAggregate;
using Social.Domain.Exceptions;

namespace Social.Application_UseCases_.Posts.CommandHandlers;

public class AddPostCommandHandler : IRequestHandler<AddPostComment,OperationResult<PostComment>>
{
    private readonly DataContext _dataContext;

    public AddPostCommandHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<OperationResult<PostComment>> Handle(AddPostComment request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<PostComment>();
        try
        {
            var post = await _dataContext.Posts.FirstOrDefaultAsync(post => post.PostId == request.PostId);
            if (post is null)
            {
                result.IsError = true;
                var error = new Error
                {
                    Code = ErrorCode.NotFound,
                    Message = $"No Post Found With This ID{request.PostId}"
                };
                result.Errors.Add(error);
                return result;
            }

            var comment = PostComment.CreatePostComment(request.PostId, request.CommentText, request.UserProfileId);
            post.AddPostComment(comment);
            _dataContext.Posts.Update(post);
            await _dataContext.SaveChangesAsync(cancellationToken);
            result.PayLoad = comment;
        }
        catch (PostCommentNotValidException ex)
        {
            result.IsError = true;
            ex.ValidationErrors.ForEach(resultError =>
            {
                var error = new Error
                {
                    Code = ErrorCode.ValidationError,
                    Message = $"{ex.Message}"
                };
                result.Errors.Add(error);
            });
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