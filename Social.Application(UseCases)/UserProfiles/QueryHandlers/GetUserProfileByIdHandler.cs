using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application_UseCases_.UserProfiles.Queries;
using Social.DataAccess;
using Social.Domain.Aggregates.UserProfileAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Social.Application_UseCases_.Enums;
using Social.Application_UseCases_.Models;

namespace Social.Application_UseCases_.UserProfiles.QueryHandlers
{
    internal class GetUserProfileByIdHandler : IRequestHandler<GetUserProfileById,OperationResult<UserProfile>>
    {
        private readonly DataContext _datactx;
        public GetUserProfileByIdHandler(DataContext dataCtx)
        {
            _datactx = dataCtx;
        }
        public async Task<OperationResult<UserProfile>> Handle(GetUserProfileById request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<UserProfile>();
            var profile  = await _datactx.UserProfiles.FirstOrDefaultAsync(
                userProfile => userProfile.UserProfileId == request.UserProfileId,cancellationToken: cancellationToken);
            
            if (profile is null) //Checking if the userprofile with this specific id exists
            {
                result.AddError(ErrorCode.NotFound,string.Format(UserProfileErrorMessage.UserProfileNotFound,request.UserProfileId));
            }

            result.PayLoad = profile;
            return result;
        }
    }
}
