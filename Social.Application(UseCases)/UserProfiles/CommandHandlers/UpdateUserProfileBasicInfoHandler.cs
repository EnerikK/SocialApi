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
using Social.Application_UseCases_.Enums;
using Social.Application_UseCases_.Models;
using Social.Domain.Exceptions;

namespace Social.Application_UseCases_.UserProfiles.CommandHandlers
{
    internal class UpdateUserProfileBasicInfoHandler : IRequestHandler<UpdateUserProfileBasicInfo,OperationResult<UserProfile>>
    {
        private readonly DataContext _datactx;
        public UpdateUserProfileBasicInfoHandler(DataContext dataCtx)
        {
            _datactx = dataCtx;
        }
        public async Task<OperationResult<UserProfile>> Handle(UpdateUserProfileBasicInfo request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<UserProfile>();

            try
            {
                var userProfile = await _datactx.UserProfiles
                    .FirstOrDefaultAsync(userProfile => userProfile.UserProfileId == request.UserProfileId,cancellationToken: cancellationToken);

                if (userProfile is null)
                {
                    result.IsError = true;
                    var error = new Error { Code = ErrorCode.NotFound, Message = $"UserProfile with Id {request.UserProfileId} not found"};
                    result.Errors.Add(error);
                    return result;
                }

                var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName
                    , request.EmailAddress, request.Phone, request.DateOfBirth, request.CurrentCity);

                userProfile.UpdateBasicInfo(basicInfo);

                _datactx.UserProfiles.Update(userProfile);
                await _datactx.SaveChangesAsync(cancellationToken);

                result.PayLoad = userProfile;
                return result;
            }
            catch (UserProfileNotValidException ex)
            {
                result.IsError = true;
                ex.ValidationErrors.ForEach(e =>
                {
                    var error = new Error
                    {
                        Code = ErrorCode.ValidationError,
                        Message = $"{ex.Message}"
                    };
                    result.Errors.Add(error);
                });
                return result;
            }
            catch (Exception ex)
            {
                var error = new Error { Code = ErrorCode.ServerError, Message = ex.Message };
                result.IsError = true;
                result.Errors.Add(error);
            }

            return result;
        }
    }
}
