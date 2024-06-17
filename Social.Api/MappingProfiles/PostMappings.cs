using AutoMapper;
using Social.Api.Contracts.Posts.Responses;
using Social.Domain.Aggregates.PostAggregate;

namespace Social.Api.MappingProfiles;

public class PostMappings : Profile
{
    public PostMappings()
    {
        CreateMap<Post, PostResponse>();
    }
}