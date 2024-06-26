using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application_UseCases_.Enums;
using Social.Application_UseCases_.Models;
using Social.Application_UseCases_.Posts.Commands;
using Social.DataAccess;
using Social.Domain.Aggregates.PostAggregate;

namespace Social.Application_UseCases_.Posts.CommandHandlers;

public class UpdatePostCommentHandler : IRequestHandler<UpdatePostComment,OperationResult<PostComment>>
{
    private readonly DataContext _dataContext;

    public UpdatePostCommentHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
        
    }
    public async Task<OperationResult<PostComment>> Handle(UpdatePostComment request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<PostComment>();
        var post = await _dataContext.Posts.Include(post => post.Comments)
            .FirstOrDefaultAsync(post => post.PostId == request.PostId, cancellationToken);

        if (post == null)
        {
            result.AddError(ErrorCode.NotFound, PostErrorMessages.PostNotFound);
            return result;
        }

        var comment = post.Comments.FirstOrDefault(com => com.CommentId == request.CommentId);

        if (comment == null)
        {
            result.AddError(ErrorCode.CommentRemovalNotAuthorized, PostErrorMessages.CommentRemovalNotAuthorized);
            return result;
        }

        comment.UpdateCommentText(request.UpdatedText);
        _dataContext.Posts.Update(post);
        await _dataContext.SaveChangesAsync(cancellationToken);

        return result;
    }
}