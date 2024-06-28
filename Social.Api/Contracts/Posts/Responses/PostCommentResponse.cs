namespace Social.Api.Contracts.Posts.Responses;

public class PostCommentResponse
{
    public string Text { get; set; }
    public string UserProfile { get; set; }
    public Guid CommentId { get; set; }
}