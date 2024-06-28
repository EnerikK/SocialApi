using AutoMapper;
using Social.Application_UseCases_.Identity.Dto_s;
using Social.Domain.Aggregates.UserProfileAggregate;

namespace Social.Api.MappingProfiles;

public class IdentityProfiles : Profile
{
    public IdentityProfiles()
    {
        //This constructor sets up a mapping profile in AutoMapper to convert a UserProfile object
        //into an IdentityUserProfileDto object by extracting specific fields from the BasicInfo sub-object.
        CreateMap<UserProfile,IdentityUserProfileDto>()
            .ForMember(dest => dest.Phone,
            opt 
                => opt.MapFrom( 
                src => src.BasicInfo.Phone)).
            ForMember(dest => dest.CurrentCity, opt 
                => opt.MapFrom(src => src.BasicInfo.CurrentCity))
            .ForMember(dest => dest.EmailAddress, opt 
                => opt.MapFrom(src => src.BasicInfo.EmailAddress))
            .ForMember(dest => dest.FirstName, opt 
                => opt.MapFrom(src => src.BasicInfo.FirstName))
            .ForMember(dest => dest.LastName, opt 
                => opt.MapFrom(src => src.BasicInfo.LastName))
            .ForMember(dest => dest.DateOfBirth, opt 
                => opt.MapFrom(src => src.BasicInfo.DateOfBirth));
            
    }
}