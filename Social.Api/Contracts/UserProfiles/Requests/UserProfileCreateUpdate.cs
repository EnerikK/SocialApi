namespace Social.Api.Contracts.UserProfile.Requests
{
    public record UserProfileCreateUpdate
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; } // maybe i remove email and phone latter 
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CurrentCity { get; set; }
    }
}
