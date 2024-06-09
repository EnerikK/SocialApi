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
    public  class UpdateUserProfileBasicInfo : IRequest<OperationResult<UserProfile>>
    {   
        public Guid UserProfileId { get; set; } //clients dont need to provide an id but in
                                                //my case if i want to update i need an id
        //i use the same variable twice because even if they are the same they change for different reasons
        //BasicInfo
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string EmailAddress { get; private set; }
        public string Phone { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string CurrentCity { get; private set; }
    }
}
