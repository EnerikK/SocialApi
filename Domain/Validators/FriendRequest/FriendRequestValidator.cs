using FluentValidation;
using FluentValidation.Results;

namespace Social.Domain.Validators.FriendRequest;

public class FriendRequestValidator : AbstractValidator<Aggregates.FriendRequests.FriendRequest>
{
    public FriendRequestValidator()
    {
        RuleFor(x => x.FriendRequestId)
            .Custom((id, context) =>
            {
                if (id == Guid.Empty)
                    context.AddFailure(new ValidationFailure("FriendRequestId",
                        "Friend request id is not a valid GUID format"));
            });
        RuleFor(x => x.DateSent).LessThanOrEqualTo(DateTime.Now);
    }
}
