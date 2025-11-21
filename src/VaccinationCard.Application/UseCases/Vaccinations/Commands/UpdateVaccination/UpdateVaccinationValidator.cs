using FluentValidation;
using VaccinationCard.Domain.Constants;

namespace VaccinationCard.Application.UseCases.Vaccinations.Commands.UpdateVaccination;

public class UpdateVaccinationValidator : AbstractValidator<UpdateVaccinationCommand>
{
    public UpdateVaccinationValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.VaccineId).GreaterThan(0);
        RuleFor(x => x.ApplicationDate).LessThanOrEqualTo(DateTime.Now);
        RuleFor(x => x.Dose)
            .Must(dose => DoseType.All.Contains(dose))
            .WithMessage($"Invalid dose. Allowed values: {string.Join(", ", DoseType.All)}");
    }
}