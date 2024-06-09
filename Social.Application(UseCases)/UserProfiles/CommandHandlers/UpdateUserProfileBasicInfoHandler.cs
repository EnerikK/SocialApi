using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application_UseCases_.UserProfiles.Commands;
using Social.DataAccess;
using Social.Domain.Aggregates.UserProfileAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Application_UseCases_.UserProfiles.CommandHandlers
{
    internal class UpdateUserProfileBasicInfoHandler : IRequestHandler<UpdateUserProfileBasicInfo>
    {
        private readonly DataContext _datactx;
        public UpdateUserProfileBasicInfoHandler(DataContext dataCtx)
        {
            _datactx = dataCtx;
        }
        public async Task<Unit> Handle(UpdateUserProfileBasicInfo request, CancellationToken cancellationToken)
        {
            var userProfile = await _datactx.UserProfiles
                .FirstOrDefaultAsync(userProfile => userProfile.UserProfileId == request.UserProfileId);

            var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName
               , request.EmailAddress, request.Phone, request.DateOfBirth, request.CurrentCity);

            userProfile.UpdateBasicInfo(basicInfo);

            _datactx.UserProfiles.Update(userProfile);
            await _datactx.SaveChangesAsync();
            return new Unit();

        }
    }
}
