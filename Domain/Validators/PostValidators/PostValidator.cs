using FluentValidation;
using Social.Domain.Aggregates.PostAggregate;

namespace Social.Domain.Validators.PostValidators;

public class PostValidator : AbstractValidator<Post>
{
    public PostValidator()
    {
        RuleFor(post => post.TextContent).NotNull().WithMessage("Post text content shouldn't be null")
            .NotEmpty().WithMessage("Post text content shouldn't be empty");
    }
}