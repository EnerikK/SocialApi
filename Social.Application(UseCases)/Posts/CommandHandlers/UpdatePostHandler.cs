using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application_UseCases_.Enums;
using Social.Application_UseCases_.Models;
using Social.Application_UseCases_.Posts.Commands;
using Social.DataAccess;
using Social.Domain.Aggregates.PostAggregate;
using Social.Domain.Exceptions;

namespace Social.Application_UseCases_.Posts.CommandHandlers;

public class UpdatePostHandler : IRequestHandler<UpdatePostText,OperationResult<Post>>
{
    private readonly DataContext _dataContext;

    public UpdatePostHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<OperationResult<Post>> Handle(UpdatePostText request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Post>();

        try
        {
            var post = await _dataContext.Posts.FirstOrDefaultAsync(post => post.PostId == request.PostId,cancellationToken: cancellationToken);
            if (post is null)
            {
                result.AddError(ErrorCode.NotFound,string.Format(PostErrorMessages.PostNotFound,request.PostId));
                return result;
            }

            if (post.UserProfileId != request.UserProfileId)
            {
                result.AddError(ErrorCode.UpdatePostNotPossible,PostErrorMessages.PostUpdateNotPossible);
                return result;
            }

            post.UpdatePostText(request.NewText);
            await _dataContext.SaveChangesAsync(cancellationToken);
            result.PayLoad = post;
        }
        catch (PostNotValidException ex)
        {
            ex.ValidationErrors.ForEach(error => result.AddError(ErrorCode.ValidationError,error));
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }
        return result;
    }
}