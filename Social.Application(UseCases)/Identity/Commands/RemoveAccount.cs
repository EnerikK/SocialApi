using MediatR;
using Social.Application_UseCases_.Models;

namespace Social.Application_UseCases_.Identity.Commands;

public class RemoveAccount : IRequest<OperationResult<bool>>
{
    public Guid IdentityUserId { get; set; }
    public Guid RequestedGuid { get; set; }
}