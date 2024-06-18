using System.ComponentModel.DataAnnotations;

namespace Social.Api.Contracts.Posts.Requests;

public class PostCommentCreate
{
    [Required] 
    public string text { get; set; }
    
    [Required]
    public string UserProfileId { get; set; }
}