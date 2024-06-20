namespace Social.Api.Contracts.Identity;

public class IdentifyUserProfile
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string Phone { get; set; }
    public string DateOfBirth { get; set; }
    public string CurrentCity { get; set; }
    public string Token { get; set; }
}