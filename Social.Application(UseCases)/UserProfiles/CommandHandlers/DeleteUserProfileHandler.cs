using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application_UseCases_.UserProfiles.Commands;
using Social.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Social.Application_UseCases_.Enums;
using Social.Application_UseCases_.Models;
using Social.Domain.Aggregates.UserProfileAggregate;

namespace Social.Application_UseCases_.UserProfiles.CommandHandlers
{
    internal class DeleteUserProfileHandler : IRequestHandler<DeleteUserProfile,OperationResult<UserProfile>>
    {
        private readonly DataContext _datactx;
        public DeleteUserProfileHandler(DataContext datactx)
        {
            _datactx = datactx;
        }
        public async Task<OperationResult<UserProfile>> Handle(DeleteUserProfile request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<UserProfile>();
            var userProfile = await _datactx.UserProfiles.FirstOrDefaultAsync(
                userprofile => userprofile.UserProfileId == request.UserProfileId,cancellationToken: cancellationToken);

            if (userProfile is null) //Checking if the userprofile with this specific id exists
            {
                result.IsError = true;
                var error = new Error { Code = ErrorCode.NotFound, Message = $"UserProfile with Id {request.UserProfileId} not found"};
                result.Errors.Add(error);
                return result;
            } 
            _datactx.UserProfiles.Remove(userProfile);
            await _datactx.SaveChangesAsync(cancellationToken);
            result.PayLoad = userProfile;
            return result;
        }
    }
}
