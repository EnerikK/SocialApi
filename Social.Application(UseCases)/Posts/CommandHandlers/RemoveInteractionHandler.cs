using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application_UseCases_.Enums;
using Social.Application_UseCases_.Identity.Commands;
using Social.Application_UseCases_.Models;
using Social.Application_UseCases_.Posts.Commands;
using Social.DataAccess;
using Social.Domain.Aggregates.PostAggregate;

namespace Social.Application_UseCases_.Posts.CommandHandlers;

public class RemoveInteractionHandler : IRequestHandler<RemoveInteraction,OperationResult<PostInteraction>>
{
    private readonly DataContext _dataContext;

    public RemoveInteractionHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public async Task<OperationResult<PostInteraction>> Handle(RemoveInteraction request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<PostInteraction>();

        try
        {
            var post = await _dataContext.Posts.Include(post => post.Interactions)
                .FirstOrDefaultAsync(post => post.PostId == request.PostId, cancellationToken);

            if (post is null)
            {
                result.AddError(ErrorCode.NotFound, string.Format(PostErrorMessages.PostNotFound, request.PostId));
                return result;
            }

            var interaction = post.Interactions.FirstOrDefault(inter => inter.InteractionId == request.InteractionId);
            if (interaction == null)
            {
                result.AddError(ErrorCode.NotFound,PostErrorMessages.PostInteractionNotFound);
                return result;
            }

            if (interaction.UserProfileId != request.UserProfile)
            {
                result.AddError(ErrorCode.InteractionRemovalNotAuthorized,PostErrorMessages.InteractionRemovalNotAuthorized);
                return result;
            }

            post.RemoveInteraction(interaction);
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