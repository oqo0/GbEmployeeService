using FluentValidation;

namespace AspWebApiGb.Models.Validators;

public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .NotEmpty()
            .Length(6, 64);
    }
}