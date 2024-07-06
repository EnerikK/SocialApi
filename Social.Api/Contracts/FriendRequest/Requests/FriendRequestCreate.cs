using System.ComponentModel.DataAnnotations;

namespace Social.Api.Contracts.FriendRequest.Requests;

public class FriendRequestCreate
{
    [Required]
    public Guid RequesterId { get; set; }
    [Required]
    public Guid ReceiverId { get; set; }
}