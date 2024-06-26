using AutoMapper;
using Social.Api.Contracts.Posts.Responses;
using Social.Domain.Aggregates.PostAggregate;
using PostInteraction = Social.Domain.Aggregates.PostAggregate.PostInteraction;

namespace Social.Api.MappingProfiles;

public class PostMappings : Profile
{
    public PostMappings()
    {
        CreateMap<Post, PostResponse>();
        CreateMap<PostComment, PostCommentResponse>();
        //This code is setting up mappings so that when you transform PostInteraction
        //objects to Social.Api.Contracts.Posts.Responses.PostInteraction objects,
        //the InteractionType is converted to its string representation and assigned to the Type property,
        //and the UserProfileId is directly mapped to the Author property.
        CreateMap<PostInteraction, Social.Api.Contracts.Posts.Responses.PostInteraction>()
            .ForMember(
                destination => destination.Type, 
                opt => opt.MapFrom(
                src => src.InteractionType.ToString())).ForMember(
                destination => destination.Author,
                opt => opt.MapFrom(
                src => src.UserProfileId));
    }
}