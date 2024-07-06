using Social.Api.Contracts.UserProfile.Responses;
using Social.Application_UseCases_.UserProfiles.Dto;

namespace Social.Api.Contracts.UserProfiles.Responses
{
    // i use records because we will want to compare to instances of userprofile for equality because when
    //i compare classes they compare references and i want a value comparison 
    public record UserProfileResponse
    {
        public Guid UserProfileId { get; set; }
        public BasicInformation BasicInfo { get; set; }
        public List<FriendRequest> FriendRequests { get; set; } = new();
        public List<Friend> Friends { get; set; } = new();
        public DateTime DateCreated { get; set; }
        public DateTime LastModified { get; set; }

        public static UserProfileResponse UserProfileDto(UserProfileDto profile)
        {
            var profileResponse = new UserProfileResponse {UserProfileId = profile.UserProfileId};
            profileResponse.BasicInfo = BasicInformation.UserInfoDto(profile.UserInfo);
            profile.FriendRequests.ForEach(friendRequest 
                => profileResponse.FriendRequests.Add(FriendRequest.FromFriendRequestDto(friendRequest)));
            profile.Friends.ForEach(friend 
                => profileResponse.Friends.Add(Friend.FromFriendDto(friend)));
            return profileResponse;
        }
    }
}
