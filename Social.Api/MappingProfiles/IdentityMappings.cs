using AutoMapper;
using Social.Api.Contracts.Identity;
using Social.Application_UseCases_.Identity.Commands;
using Social.Application_UseCases_.Identity.Dto_s;

namespace Social.Api.MappingProfiles;

public class IdentityMappings : Profile
{
    public IdentityMappings()
    {
        CreateMap<UserRegistration, RegisterIdentify>();
        CreateMap<Login, LoginCommand>();
        CreateMap<IdentityUserProfileDto, IdentityUserProfile>();
    }
        
}