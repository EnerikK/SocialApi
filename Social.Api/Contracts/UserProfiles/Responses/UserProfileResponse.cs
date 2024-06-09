using Social.Domain.Aggregates.UserProfileAggregate;

namespace Social.Api.Contracts.UserProfile.Responses
{
    // we use records because we will want to compare to instances of userprofile for equality because when
    //we compare classes they compare references and i want a value comparison 
    public record UserProfileResponse
    {
        public Guid UserProfileId { get; set; }
        public BasicInformation BasicInfo { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastModified { get; set; }
    }
}
