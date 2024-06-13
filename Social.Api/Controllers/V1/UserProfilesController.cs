using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Social.Api.Contracts.Common;
using Social.Api.Contracts.UserProfile.Requests;
using Social.Api.Contracts.UserProfile.Responses;
using Social.Api.Filters;
using Social.Application_UseCases_.Enums;
using Social.Application_UseCases_.UserProfiles.Commands;
using Social.Application_UseCases_.UserProfiles.Queries;

namespace Social.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route(ApiRoutes.BaseRoute)]
    [ApiController]
    public class UserProfilesController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public UserProfilesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProfiles()
        {
            //throw new NotImplementedException("Method Not Implemented");
            
            var query = new GetAllUserProfiles();
            //now send this to the mediator
            var response = await _mediator.Send(query);
            var profiles = _mapper.Map<List<UserProfileResponse>>(response.PayLoad);
            return Ok(profiles);
        }
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateUserProfile([FromBody] UserProfileCreateUpdate profile)
        {
            var command = _mapper.Map<CreateUserCommand>(profile);
            var response = await _mediator.Send(command);
            var userProfile = _mapper.Map<UserProfileResponse>(response.PayLoad);
            return CreatedAtAction(nameof(GetUserProfileById), new {id = userProfile.UserProfileId} , userProfile);
        }
        [Route(ApiRoutes.UserProfiles.IdRoute)]
        [HttpGet]
        [ValidateGuid("id")]
        public async Task<IActionResult> GetUserProfileById(string id)
        {
            var query = new GetUserProfileById {UserProfileId = Guid.Parse(id)};
            var response = await _mediator.Send(query);

            if (response.IsError) return HandleErrorResponse(response.Errors);
            
            var userProfile = _mapper.Map<UserProfileResponse>(response.PayLoad);
            return Ok(userProfile);
        }
        [HttpPatch]
        [Route(ApiRoutes.UserProfiles.IdRoute)]
        [ValidateModel]
        [ValidateGuid("id")]
        public async Task<IActionResult> UpdateUserProfile(string id, UserProfileCreateUpdate updateProfile)
        {
            var command = _mapper.Map<UpdateUserProfileBasicInfo>(updateProfile);
            command.UserProfileId = Guid.Parse(id);
            var response = await _mediator.Send(command);

            if (response.IsError) return HandleErrorResponse(response.Errors);
            return NoContent();
        }
        [HttpDelete]
        [Route(ApiRoutes.UserProfiles.IdRoute)]
        [ValidateGuid("id")]
        public async Task<IActionResult> DeleteUserProfile(string id)
        {
            var command = new DeleteUserProfile() { UserProfileId = Guid.Parse(id)};
            var response = await _mediator.Send(command); // send the command down the mediator pipeline
                                                          // will execute and return a response
            if (response.IsError) return HandleErrorResponse(response.Errors);
            return NoContent();
        }
    }
}
