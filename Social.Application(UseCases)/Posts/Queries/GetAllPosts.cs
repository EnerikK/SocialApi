using MediatR;
using Social.Application_UseCases_.Models;
using Social.Domain.Aggregates.PostAggregate;

namespace Social.Application_UseCases_.Posts.Queries;

public class GetAllPosts : IRequest<OperationResult<List<Post>>>
{
    
}