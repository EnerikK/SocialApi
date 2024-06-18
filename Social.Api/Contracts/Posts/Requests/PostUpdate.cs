using System.ComponentModel.DataAnnotations;

namespace Social.Api.Contracts.Posts.Requests;

public class PostUpdate
{
    [Required]
    public string text { get; set; }
}