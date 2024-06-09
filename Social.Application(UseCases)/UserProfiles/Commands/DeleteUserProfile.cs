using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Application_UseCases_.UserProfiles.Commands
{
    public class DeleteUserProfile : IRequest
    {
        public Guid UserProfileId { get; set; }
    }
}
