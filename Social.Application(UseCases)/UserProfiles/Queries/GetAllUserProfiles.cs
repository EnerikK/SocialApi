using MediatR;
using Social.Domain.Aggregates.UserProfileAggregate;


namespace Social.Application_UseCases_.UserProfiles.Queries
{
    public class GetAllUserProfiles : IRequest<IEnumerable<UserProfile>>
    {

    }
}
