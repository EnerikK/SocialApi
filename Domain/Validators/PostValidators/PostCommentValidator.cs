using FluentValidation;
using Social.Domain.Aggregates.PostAggregate;

namespace Social.Domain.Validators.PostValidators;

public class PostCommentValidator : AbstractValidator<PostComment>
{
    public PostCommentValidator()
    {
        RuleFor( postComment => postComment.Text).NotNull().WithMessage("Comment text shouldn't be null")
            .NotEmpty().WithMessage("Comment text shouldn't be empty ")
            .MaximumLength(1000)
            .MinimumLength(1);
    }
}