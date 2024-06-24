using MediatR;
using Social.Application_UseCases_.Enums;
using Social.Application_UseCases_.Models;
using Social.Application_UseCases_.Posts.Commands;
using Social.DataAccess;
using Social.Domain.Aggregates.PostAggregate;
using Social.Domain.Exceptions;

namespace Social.Application_UseCases_.Posts.CommandHandlers;

public class CreatePostHandler : IRequestHandler<CreatePost,OperationResult<Post>>
{
    private readonly DataContext _dataContext;

    public CreatePostHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<OperationResult<Post>> Handle(CreatePost request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Post>();
        try
        {
            var post = Post.CreatePost(request.UserProfileId, request.TextContent);
            _dataContext.Posts.Add(post);
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