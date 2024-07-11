namespace Social.Application_UseCases_.Enums;

public enum ErrorCode
{
    NotFound = 404,
    ServerError = 500,
    
    ValidationError = 101,
    FriendRequestValidationError = 102,

    IdentityUserAlreadyExists = 102,
    
    IdentityCreationFailed = 202,
    IdentityUserDoesNotExist = 203,
    
    IncorrectPassword = 300,
    UpdatePostNotPossible = 301,
    DeletePostNotPossible = 302,
    UnauthorizedAccountRemoval = 303,
    InteractionRemovalNotAuthorized = 304,
    CommentRemovalNotAuthorized = 305,
    FriendRequestAcceptNotPossible = 306,
    FriendRequestRejectNotPossible = 307,
    DatabaseOperationException = 308,
    
    UnknownError = 1,

}