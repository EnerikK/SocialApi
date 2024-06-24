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
                result.AddError(ErrorCode.NotFound,string.Format(PostErrorMessages.PostNotFound,request.PostId));
            }

            var comment = PostComment.CreatePostComment(request.PostId, request.CommentText, request.UserProfileId);
            post.AddPostComment(comment);
            _dataContext.Posts.Update(post);
            await _dataContext.SaveChangesAsync(cancellationToken);
            result.PayLoad = comment;
        }
        catch (PostCommentNotValidException ex)
        {
            ex.ValidationErrors.ForEach(error => result.AddError(ErrorCode.ValidationError , error));
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }
        return result;
    }
}