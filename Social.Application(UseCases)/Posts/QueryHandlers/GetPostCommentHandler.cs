using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application_UseCases_.Enums;
using Social.Application_UseCases_.Models;
using Social.Application_UseCases_.Posts.Queries;
using Social.DataAccess;
using Social.Domain.Aggregates.PostAggregate;

namespace Social.Application_UseCases_.Posts.QueryHandlers;

public class GetPostCommentHandler : IRequestHandler<GetPostComments,OperationResult<List<PostComment>>>
{
    private readonly DataContext _dataContext;

    public GetPostCommentHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<OperationResult<List<PostComment>>> Handle(GetPostComments request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<List<PostComment>>();
        try
        {
            var post = await _dataContext.Posts.Include(post => post.Comments)
                .FirstOrDefaultAsync(post => post.PostId == request.PostId);

            result.PayLoad = post.Comments.ToList();
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }

        return result;
    }
}