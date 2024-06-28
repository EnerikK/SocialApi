using System.Security.Claims;
using MediatR;
using Social.Application_UseCases_.Identity.Dto_s;
using Social.Application_UseCases_.Models;

namespace Social.Application_UseCases_.Identity.Query;

public class GetCurrentUser : IRequest<OperationResult<IdentityUserProfileDto>>
{
    public Guid UserProfileId { get; set; }
    public ClaimsPrincipal ClaimsPrincipal { get; set; }
}