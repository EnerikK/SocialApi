using MediatR;
using Social.Application_UseCases_.Identity.Dto_s;
using Social.Application_UseCases_.Models;

namespace Social.Application_UseCases_.Identity.Commands;

public class RegisterIdentify : IRequest<OperationResult<IdentityUserProfileDto>>
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Phone { get; set; }
    public string CurrentCity { get; set; }
}