using Social.Application_UseCases_.UserProfiles.Dto;

namespace Social.Api.Contracts.UserProfile.Responses;

public class Friend
{
    public Guid FriendShipId { get; set; }
    public string? FriendFullName { get; set; }
    public string? City { get; set; }

    public static Friend FromFriendDto(FriendDto friendDto)
    {
        return new Friend
        {
            FriendShipId = friendDto.FriendshipId,
            FriendFullName = friendDto.FriendFullName,
            City = friendDto.City
        };
    }
}