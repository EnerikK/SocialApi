using MediatR;
using Social.Application_UseCases_.Models;
using Social.Domain.Aggregates.PostAggregate;

namespace Social.Application_UseCases_.Posts.Queries;

public class GetPostInteractions : IRequest<OperationResult<List<PostInteraction>>>
{
    public Guid PostId { get; set; }
}