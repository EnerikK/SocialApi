using Social.Domain.Aggregates.FriendRequests;

namespace Social.Application_UseCases_.UserProfiles.Dto;

public class FriendDto
{
    public Guid FriendshipId { get; set; }
    public string? FriendFullName { get; set; }
    public string? City { get; set; }

    public static FriendDto FromFriendship(FriendStatus friendship, Guid currentUser)
    {
        return new FriendDto
        {
            FriendshipId = friendship.FriendshipId,
            FriendFullName = GetFriendFullName(friendship, currentUser),
            City = GetFriendCity(friendship, currentUser)
        };
    }

    private static string GetFriendFullName(FriendStatus friendship, Guid currentUser)
    {
        if (friendship.FirstFriendUserProfileId == currentUser)
            return $"{friendship?.SecondFriend?.BasicInfo.FirstName} {friendship?.SecondFriend?.BasicInfo.LastName}";

        return $"{friendship?.FirstFriend?.BasicInfo.FirstName} {friendship?.FirstFriend?.BasicInfo.LastName}";
    }

    private static string GetFriendCity(FriendStatus friendship, Guid currentUser)
    {
        if (friendship.FirstFriendUserProfileId == currentUser)
            return friendship?.SecondFriend?.BasicInfo?.CurrentCity!;

        return friendship?.FirstFriend?.BasicInfo?.CurrentCity!;
    }
}