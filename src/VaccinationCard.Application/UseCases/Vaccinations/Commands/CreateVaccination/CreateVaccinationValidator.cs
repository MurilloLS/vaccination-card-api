using FluentValidation;
using VaccinationCard.Domain.Constants;

namespace VaccinationCard.Application.UseCases.Vaccinations.Commands.CreateVaccination;

public class CreateVaccinationValidator : AbstractValidator<CreateVaccinationCommand>
{
    public CreateVaccinationValidator()
    {
        RuleFor(x => x.PersonId).GreaterThan(0);
        RuleFor(x => x.VaccineId).GreaterThan(0);

        RuleFor(x => x.ApplicationDate)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Application date cannot be in the future.");

        // Valida se a dose está na lista permitida (D1, D2, R1...)
        RuleFor(x => x.Dose)
            .Must(dose => DoseType.All.Contains(dose))
            .WithMessage($"Invalid dose. Allowed values: {string.Join(", ", DoseType.All)}");
    }
}