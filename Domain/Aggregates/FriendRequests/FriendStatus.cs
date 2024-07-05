using Social.Domain.Aggregates.UserProfileAggregate;

namespace Social.Domain.Aggregates.FriendRequests;

public class FriendStatus
{
    internal FriendStatus() { }
    public Guid FriendshipId { get; internal set; }
    public Guid? FirstFriendUserProfileId { get; internal set; }
    public UserProfile? FirstFriend { get; internal set; }
    public Guid? SecondFriendUserProfileId { get; internal set; }
    public UserProfile? SecondFriend { get; internal set; }
    public DateTime DateEstablished { get; internal set; }
    public FriendRequestStatus FriendshipStatus { get; internal set; }

    public void Unfriend()
    {
        FriendshipStatus = FriendRequestStatus.Inactive;
    }
}