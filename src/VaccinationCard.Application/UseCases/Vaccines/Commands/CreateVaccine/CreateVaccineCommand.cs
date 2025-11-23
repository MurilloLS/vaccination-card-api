using MediatR;
using VaccinationCard.Application.DTOs;

namespace VaccinationCard.Application.UseCases.Vaccines.Commands.CreateVaccine;

public record CreateVaccineCommand(string Name, int CategoryId, int MaxDoses) : IRequest<VaccineDto>;