using MediatR;
using Social.Domain.Aggregates.UserProfileAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Social.Application_UseCases_.Models;
using Social.Application_UseCases_.UserProfiles.Dto;

namespace Social.Application_UseCases_.UserProfiles.Queries
{
    public class GetUserProfileById : IRequest<OperationResult<UserProfileDto>>
    {
        public Guid UserProfileId { get; set; }
    }
}
