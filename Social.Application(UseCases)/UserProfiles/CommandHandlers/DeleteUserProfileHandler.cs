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

namespace Social.Application_UseCases_.UserProfiles.CommandHandlers
{
    internal class DeleteUserProfileHandler : IRequestHandler<DeleteUserProfile>
    {
        private readonly DataContext _datactx;
        public DeleteUserProfileHandler(DataContext datactx)
        {
            _datactx = datactx;
        }
        public async Task<Unit> Handle(DeleteUserProfile request, CancellationToken cancellationToken)
        {
            var userProfile = await _datactx.UserProfiles.FirstOrDefaultAsync(
                userprofile => userprofile.UserProfileId == request.UserProfileId);

            _datactx.UserProfiles.Remove(userProfile);
            await _datactx.SaveChangesAsync();
            return new Unit();
        }
    }
}
