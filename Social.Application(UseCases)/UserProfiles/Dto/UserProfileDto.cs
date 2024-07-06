using Social.Domain.Aggregates.FriendRequests;
using Social.Domain.Aggregates.UserProfileAggregate;

namespace Social.Application_UseCases_.UserProfiles.Dto;

public class UserProfileDto
{
    public Guid UserProfileId { get; set; }
    public UserInfoDto? UserInfo { get; set; }
    public List<FriendRequestDto> FriendRequests { get; set; } = new();
    public List<FriendDto> Friends { get; set; } = new();

    public static UserProfileDto FromUserProfile(UserProfile profile,
        List<Domain.Aggregates.FriendRequests.FriendRequest> friendRequests, List<FriendStatus> friendships)
    {
        var userProfileDto = new UserProfileDto { UserProfileId = profile.UserProfileId};
        userProfileDto.UserInfo = UserInfoDto.FromBasicInfo(profile.BasicInfo);
        friendRequests.ForEach(fr 
            => userProfileDto.FriendRequests.Add(FriendRequestDto.FriendRequest(fr)));
        friendships.ForEach(f 
            => userProfileDto.Friends.Add(FriendDto.FromFriendship(f, userProfileDto.UserProfileId)));

        return userProfileDto;
    }
}