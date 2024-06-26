using MediatR;
using Social.Application_UseCases_.Models;
using Social.Domain.Aggregates.PostAggregate;

namespace Social.Application_UseCases_.Posts.Commands;

public class RemoveInteraction : IRequest<OperationResult<PostInteraction>>
{
    public Guid PostId { get; set; }
    public Guid InteractionId { get; set; }
    public Guid UserProfile { get; set; }
}