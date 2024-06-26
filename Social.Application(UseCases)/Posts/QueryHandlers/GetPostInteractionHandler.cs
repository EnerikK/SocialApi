using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application_UseCases_.Enums;
using Social.Application_UseCases_.Models;
using Social.Application_UseCases_.Posts.CommandHandlers;
using Social.Application_UseCases_.Posts.Queries;
using Social.DataAccess;
using Social.Domain.Aggregates.PostAggregate;

namespace Social.Application_UseCases_.Posts.QueryHandlers;

public class GetPostInteractionHandler : IRequestHandler<GetPostInteractions,OperationResult<List<PostInteraction>>>
{
    private readonly DataContext _dataContext;
    public GetPostInteractionHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<OperationResult<List<PostInteraction>>> Handle(GetPostInteractions request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<List<PostInteraction>>();

        try
        {
            var post = await _dataContext.Posts
                .Include(post => post.Interactions)
                .ThenInclude(incl => incl.UserProfileId)
                .FirstOrDefaultAsync(post => post.PostId == request.PostId, cancellationToken);

            if (post == null)
            {
                result.AddError(ErrorCode.NotFound, PostErrorMessages.PostNotFound);
                return result;
                
            }
            result.PayLoad = post.Interactions.ToList();
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }

        return result;
    }
}