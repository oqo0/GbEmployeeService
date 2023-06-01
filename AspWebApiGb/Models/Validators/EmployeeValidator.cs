using AspWebApiGb.Models.Dto;
using FluentValidation;

namespace AspWebApiGb.Models.Validators;

public class EmployeeValidator : AbstractValidator<EmployeeDto>
{
    public EmployeeValidator()
    {
        RuleFor(x => x.Patronymic)
            .NotNull()
            .NotEmpty()
            .Length(3, 128);
        
        RuleFor(x => x.FirstName)
            .NotNull()
            .NotEmpty()
            .Length(3, 128);
        
        RuleFor(x => x.Surname)
            .NotNull()
            .NotEmpty()
            .Length(3, 128);
        
        RuleFor(x => x.Salary)
            .NotNull()
            .NotEmpty()
            .GreaterThanOrEqualTo(1);
    }
}