using MediatR;
using VaccinationCard.Application.DTOs;

namespace VaccinationCard.Application.UseCases.Vaccines.Commands.UpdateVaccine;

public record UpdateVaccineCommand(int Id, string Name, int CategoryId, int MaxDoses) : IRequest<VaccineDto?>;