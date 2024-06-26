using MediatR;
using Social.Application_UseCases_.Models;
using Social.Domain.Aggregates.PostAggregate;

namespace Social.Application_UseCases_.Posts.Commands;

public class RemoveCommentFromPost : IRequest<OperationResult<PostComment>>
{
    public Guid UserProfileId { get; set; }
    public Guid PostId { get; set; }
    public Guid CommentId { get; set; }
}