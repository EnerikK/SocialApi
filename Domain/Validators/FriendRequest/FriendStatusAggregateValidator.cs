using FluentValidation.Results;
using Social.Domain.Exceptions;

namespace Social.Domain.Validators.FriendRequest;

public class FriendStatusAggregateValidator
{
    public static void ValidateFriendRequest(Aggregates.FriendRequests.FriendRequest friendRequest)
    {
        var validator = new FriendRequestValidator();
        var validationResult = validator.Validate(friendRequest);

        if (!validationResult.IsValid) ThrowNotValidException<FriendRequestValidationException>(validationResult.Errors);
    }

    private static void ThrowNotValidException<T>(List<ValidationFailure> errors) 
        where T : DomainModelInvalidException
    {
        var exception = new FriendRequestValidationException("Friend request is not  valid");
        errors
            .ForEach(e => exception.ValidationErrors.Add(e.ErrorMessage));
        throw exception;
    }
}