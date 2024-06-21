using System.ComponentModel.DataAnnotations;

namespace Social.Api.Contracts.Posts.Requests;

public class PostCreate
{
    [Required]
    public string TextContent { get;  set; }
}