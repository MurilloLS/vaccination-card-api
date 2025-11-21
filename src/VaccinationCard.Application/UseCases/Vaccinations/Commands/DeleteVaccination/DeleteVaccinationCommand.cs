using MediatR;
using VaccinationCard.Application.DTOs;

namespace VaccinationCard.Application.UseCases.Vaccinations.Commands.DeleteVaccination;

public record DeleteVaccinationCommand(int Id) : IRequest<VaccinationDto?>;