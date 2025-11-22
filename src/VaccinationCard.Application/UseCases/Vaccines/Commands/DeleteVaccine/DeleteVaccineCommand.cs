using MediatR;
using VaccinationCard.Application.DTOs;

namespace VaccinationCard.Application.UseCases.Vaccines.Commands.DeleteVaccine;

public record DeleteVaccineCommand(int Id) : IRequest<VaccineDto?>;