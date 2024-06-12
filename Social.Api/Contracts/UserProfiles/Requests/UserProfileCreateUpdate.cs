using System.ComponentModel.DataAnnotations;

namespace Social.Api.Contracts.UserProfile.Requests
{
    public record UserProfileCreateUpdate
    {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string FirstName { get; set; }
        
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string LastName { get; set; }
        
        [EmailAddress]
        [Required]
        public string EmailAddress { get; set; } // maybe i remove email and phone latter 
        
        public string Phone { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        public string CurrentCity { get; set; }
    }
}
