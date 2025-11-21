using MediatR;
using VaccinationCard.Application.DTOs;

namespace VaccinationCard.Application.UseCases.Vaccinations.Queries.GetVaccinationById;

public record GetVaccinationByIdQuery(int Id) : IRequest<VaccinationDto?>;