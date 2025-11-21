using MediatR;
using VaccinationCard.Application.DTOs;

namespace VaccinationCard.Application.UseCases.Vaccinations.Commands.CreateVaccination;

public record CreateVaccinationCommand(
    int PersonId,
    int VaccineId,
    string Dose,
    DateTime ApplicationDate
) : IRequest<VaccinationDto>;