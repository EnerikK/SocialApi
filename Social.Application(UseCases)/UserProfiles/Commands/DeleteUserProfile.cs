using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Social.Application_UseCases_.Models;
using Social.Domain.Aggregates.UserProfileAggregate;

namespace Social.Application_UseCases_.UserProfiles.Commands
{
    public class DeleteUserProfile : IRequest<OperationResult<UserProfile>>
    {
        public Guid UserProfileId { get; set; }
    }
}
