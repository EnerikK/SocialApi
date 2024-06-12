using MediatR;
using Social.Application_UseCases_.Models;
using Social.Domain.Aggregates.UserProfileAggregate;


namespace Social.Application_UseCases_.UserProfiles.Queries
{
    public class GetAllUserProfiles : IRequest<OperationResult<IEnumerable<UserProfile>>>
    {

    }
}
