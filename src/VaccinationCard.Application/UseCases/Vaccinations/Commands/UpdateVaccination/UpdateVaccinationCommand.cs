using MediatR;
using VaccinationCard.Application.DTOs;

namespace VaccinationCard.Application.UseCases.Vaccinations.Commands.UpdateVaccination;

public record UpdateVaccinationCommand(
    int Id,
    int VaccineId,
    string Dose,
    DateTime ApplicationDate
) : IRequest<VaccinationDto?>;