using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application_UseCases_.Enums;
using Social.Application_UseCases_.Models;
using Social.Application_UseCases_.Posts.Commands;
using Social.DataAccess;
using Social.Domain.Aggregates.PostAggregate;

namespace Social.Application_UseCases_.Posts.CommandHandlers;

public class AddInteractionHandler : IRequestHandler<AddInteraction,OperationResult<PostInteraction>>
{
    private readonly DataContext _dataContext;
    
    public AddInteractionHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<OperationResult<PostInteraction>> Handle(AddInteraction request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<PostInteraction>();
        try
        {
            var post = await _dataContext.Posts.Include(post => post.Interactions)
                .FirstOrDefaultAsync(post => post.PostId == request.PostId, cancellationToken);

            if (post == null)
            {
                result.AddError(ErrorCode.NotFound, PostErrorMessages.PostNotFound);
                return result;
            }
            var interaction =
                PostInteraction.CreatePostInteraction(request.PostId, request.Type,request.UserProfileId);

            post.AddInteraction(interaction);
            _dataContext.Posts.Update(post);
            await _dataContext.SaveChangesAsync(cancellationToken);
            result.PayLoad = interaction;
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }
        return result;
    }
}