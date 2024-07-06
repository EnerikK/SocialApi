using Social.Application_UseCases_.UserProfiles.Dto;

namespace Social.Api.Contracts.UserProfiles.Responses;

public class FriendRequest
{
    public Guid FriendRequestId { get; set; }
    public string? RequesterFullname { get; set; }
    public string? City { get; set; }

    public static FriendRequest FromFriendRequestDto(FriendRequestDto requestDto)
    {
        return new FriendRequest
        {
            FriendRequestId = requestDto.FriendRequestId,
            RequesterFullname = requestDto.RequesterFullName,
            City = requestDto.City
        };
    }
}
