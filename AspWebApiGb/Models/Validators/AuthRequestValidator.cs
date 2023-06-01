using AspWebApiGb.Models.Requests;
using FluentValidation;

namespace AspWebApiGb.Models.Validators;

public class AuthRequestValidator : AbstractValidator<AuthRequest>
{
    public AuthRequestValidator()
    {
        RuleFor(x => x.Login)
            .NotNull()
            .NotEmpty()
            .Length(7, 255)
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotNull()
            .NotEmpty()
            .Length(6, 64);
    }
}