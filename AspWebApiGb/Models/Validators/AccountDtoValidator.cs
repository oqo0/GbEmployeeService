using AspWebApiGb.Models.Dto;
using FluentValidation;

namespace AspWebApiGb.Models.Validators;

public class AccountDtoValidator : AbstractValidator<AccountDto>
{
    public AccountDtoValidator()
    {
        RuleFor(x => x.EMail)
            .NotNull()
            .NotEmpty()
            .Length(7, 255)
            .EmailAddress();

        RuleFor(x => x.FirstName)
            .NotNull()
            .NotEmpty()
            .Length(3, 128);
        
        RuleFor(x => x.SecondName)
            .NotNull()
            .NotEmpty()
            .Length(3, 128);
        
        RuleFor(x => x.LastName)
            .NotNull()
            .NotEmpty()
            .Length(3, 128);
    }
}