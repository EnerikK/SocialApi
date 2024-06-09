using AutoMapper;
using Social.Api.Contracts.UserProfile.Requests;
using Social.Api.Contracts.UserProfile.Responses;
using Social.Application_UseCases_.UserProfiles.Commands;
using Social.Domain.Aggregates.UserProfileAggregate;

namespace Social.Api.MappingProfiles
{
    public class UserProfileMappings : Profile
    {
        public UserProfileMappings()
        {
            CreateMap<UserProfileCreateUpdate, CreateUserCommand>();
            CreateMap<UserProfileCreateUpdate, UpdateUserProfileBasicInfo>();
            CreateMap<UserProfile, UserProfileResponse>();
            CreateMap<BasicInfo, BasicInformation>();
        }
    }
}
