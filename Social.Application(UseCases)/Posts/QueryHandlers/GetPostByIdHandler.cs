using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application_UseCases_.Enums;
using Social.Application_UseCases_.Models;
using Social.Application_UseCases_.Posts.Queries;
using Social.DataAccess;
using Social.Domain.Aggregates.PostAggregate;

namespace Social.Application_UseCases_.Posts.QueryHandlers;

public class GetPostByIdHandler : IRequestHandler<GetPostById,OperationResult<Post>>
{
    private readonly DataContext _dataContext;
    public GetPostByIdHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<OperationResult<Post>> Handle(GetPostById request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Post>();
        var post  = await _dataContext.Posts.FirstOrDefaultAsync(
            post => post.PostId == request.PostId);
            
        if (post is null) //Checking if the userprofile with this specific id exists
        {
            result.IsError = true;
            var error = new Error { Code = ErrorCode.NotFound, Message = $"Post with Id {request.PostId} not found"};
            result.Errors.Add(error);
            return result;
        }

        result.PayLoad = post;
        return result;
    }
}