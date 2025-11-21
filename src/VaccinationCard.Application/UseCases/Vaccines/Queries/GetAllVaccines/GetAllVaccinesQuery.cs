using MediatR;
using VaccinationCard.Application.DTOs;

namespace VaccinationCard.Application.UseCases.Vaccines.Queries.GetAllVaccines;

public record GetAllVaccinesQuery() : IRequest<IEnumerable<VaccineDto>>;