using FluentValidation;

namespace VaccinationCard.Application.UseCases.Persons.Commands.UpdatePerson;

public class UpdatePersonValidator : AbstractValidator<UpdatePersonCommand>
{
    public UpdatePersonValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(150);

        RuleFor(x => x.Age)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Gender)
            .NotEmpty()
            .Length(1);
    }
}