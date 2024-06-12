using MediatR;
using Social.Domain.Aggregates.UserProfileAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Social.Application_UseCases_.Models;


namespace Social.Application_UseCases_.UserProfiles.Commands
{
    public class CreateUserCommand : IRequest<OperationResult<UserProfile>> // use automapper to map from contract to the command
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; } // maybe i remove email and phone latter 
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CurrentCity { get; set; }
    }
}
