using AutoMapper;
using Social.Api.Contracts.Posts.Responses;
using Social.Api.Contracts.UserProfile.Requests;
using Social.Api.Contracts.UserProfile.Responses;
using Social.Application_UseCases_.UserProfiles.Commands;
using Social.Domain.Aggregates.PostAggregate;
using Social.Domain.Aggregates.UserProfileAggregate;

namespace Social.Api.MappingProfiles
{
    public class UserProfileMappings : Profile
    {
        public UserProfileMappings()
        {
            CreateMap<UserProfileCreateUpdate, UpdateUserProfileBasicInfo>();
            CreateMap<UserProfile, UserProfileResponse>();
            CreateMap<BasicInfo, BasicInformation>();
            /*The AutoMapper configuration is set up to transform UserProfile objects into InteractionUser objects by:
            Combining FirstName and LastName from BasicInfo to form FullName.
            Mapping CurrentCity from BasicInfo to City.
            This makes it easier to convert detailed user profile data into a simpler form suitable for specific interactions,
            such as displaying user information in a list or passing simplified data through an API.*/
            CreateMap<UserProfile, InteractionUser>()
                .ForMember(destination => destination.FullName,
                    options
                        => options.MapFrom(source => source.BasicInfo.FirstName + " " + source.BasicInfo.LastName))
                .ForMember(destination => destination.City,
                    options =>
                        options.MapFrom(source => source.BasicInfo.CurrentCity));
        }
    }
}
