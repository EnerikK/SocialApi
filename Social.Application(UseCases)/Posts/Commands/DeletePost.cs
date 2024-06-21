using MediatR;
using Social.Application_UseCases_.Models;
using Social.Domain.Aggregates.PostAggregate;

namespace Social.Application_UseCases_.Posts.Commands;

public class DeletePost : IRequest<OperationResult<Post>>
{
    public Guid PostId { get; set; }
    public Guid UserProfileId { get; set; }
}