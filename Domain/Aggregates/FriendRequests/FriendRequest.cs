using Social.Domain.Aggregates.UserProfileAggregate;
using Social.Domain.Validators.FriendRequest;

namespace Social.Domain.Aggregates.FriendRequests;

public class FriendRequest
{
    private FriendRequest() {}
    public Guid FriendRequestId { get; private set; }
    public Guid? RequesterUserProfileId { get; private set; }
    public UserProfile? Requester { get; private set; }
    public Guid? ReceiverUserProfileId { get; private set; }
    public UserProfile? Receiver { get; private set; }
    public DateTime DateSent { get; private set; }
    public DateTime DateResponded { get; private set; }
    public ResponseType Response { get; private set; }

    public static FriendRequest CreateFriendRequest(Guid friendRequestId, Guid requesterId, Guid receiverId,
        DateTime dateSent)
    {
        var friendRequest = new FriendRequest();
        friendRequest.FriendRequestId = friendRequestId;
        friendRequest.RequesterUserProfileId = requesterId;
        friendRequest.ReceiverUserProfileId = receiverId;
        friendRequest.DateSent = dateSent;
        friendRequest.Response = ResponseType.Pending;

        FriendStatusAggregateValidator.ValidateFriendRequest(friendRequest);

        return friendRequest;
    }
    public FriendStatus? AcceptFriendRequest(Guid friendshipId)
    {
        var friendship = new FriendStatus()
        {
            FriendshipId = friendshipId,
            FirstFriendUserProfileId = RequesterUserProfileId,
            SecondFriendUserProfileId = ReceiverUserProfileId,
            DateEstablished = DateTime.UtcNow,
            FriendshipStatus = FriendRequestStatus.Active
        };

        Response = ResponseType.Accepted;
        DateResponded = DateTime.UtcNow;
        return friendship;
    }

    public void RejectFriendRequest()
    {
        Response = ResponseType.Declined;
        DateResponded = DateTime.UtcNow;
    }
}