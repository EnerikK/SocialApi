using Social.Domain.Aggregates.FriendRequests;

namespace Social.Application_UseCases_.UserProfiles.Dto;

public class FriendRequestDto
{
    public Guid FriendRequestId { get; set; }
    public string? RequesterFullName { get; set; }
    public string? City { get; set; }

    public static FriendRequestDto FriendRequest(Domain.Aggregates.FriendRequests.FriendRequest request)
    {
        return new FriendRequestDto
        {
            FriendRequestId = request.FriendRequestId,
            RequesterFullName = $"{request.Requester.BasicInfo.FirstName}{request.Requester.BasicInfo.LastName}",
            City = request.Requester.BasicInfo.CurrentCity
        };
    }
}