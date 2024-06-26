namespace Social.Application_UseCases_.Enums;

public enum ErrorCode
{
    NotFound = 1,
    ServerError = 2,
    ValidationError = 3,
    IdentityUserAlreadyExists = 4,
    IdentityCreationFailed = 5,
    IdentityUserDoesNotExist = 6,
    IncorrectPassword = 7,
    UpdatePostNotPossible = 8,
    DeletePostNotPossible = 9,
    UnauthorizedAccountRemoval = 10,
    InteractionRemovalNotAuthorized = 11,
    UnknownError = 12,

}