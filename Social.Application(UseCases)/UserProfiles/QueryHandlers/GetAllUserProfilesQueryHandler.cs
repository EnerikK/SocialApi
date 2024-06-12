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
using Social.Application_UseCases_.Models;

namespace Social.Application_UseCases_.UserProfiles.QueryHandlers
{
    internal class GetAllUserProfilesQueryHandler : IRequestHandler<GetAllUserProfiles, OperationResult<IEnumerable<UserProfile>>>
    {
        private readonly DataContext _Datactx;
        public GetAllUserProfilesQueryHandler(DataContext Datactx)
        {
            _Datactx = Datactx;
        }
        public async Task<OperationResult<IEnumerable<UserProfile>>> Handle(GetAllUserProfiles request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IEnumerable<UserProfile>>();
            result.PayLoad = await _Datactx.UserProfiles.ToListAsync();
            return result;
        }
    }
}
