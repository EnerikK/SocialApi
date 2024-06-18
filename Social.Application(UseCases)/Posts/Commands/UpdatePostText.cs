using MediatR;
using Social.Application_UseCases_.Models;
using Social.Domain.Aggregates.PostAggregate;

namespace Social.Application_UseCases_.Posts.Commands;

public class UpdatePostText : IRequest<OperationResult<Post>>
{
    public string NewText { get; set; }
    public Guid PostId { get; set; }
}