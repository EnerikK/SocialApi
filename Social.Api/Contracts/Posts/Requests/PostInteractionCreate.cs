using System.ComponentModel.DataAnnotations;
using Social.Domain.Aggregates.PostAggregate;

namespace Social.Api.Contracts.Posts.Requests;

public class PostInteractionCreate
{
    [Required]
    public InteractionTypes Type { get; set; }
}