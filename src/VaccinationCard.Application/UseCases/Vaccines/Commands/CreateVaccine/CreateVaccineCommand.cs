using MediatR;
using VaccinationCard.Application.DTOs;

namespace VaccinationCard.Application.UseCases.Vaccines.Commands.CreateVaccine;

// Recebe Nome e ID da Categoria (Ex: 1 para Nacional)
public record CreateVaccineCommand(string Name, int CategoryId) : IRequest<VaccineDto>;