using MediatR;
using VaccinationCard.Application.DTOs;

namespace VaccinationCard.Application.UseCases.Persons.Commands.UpdatePerson;

// Recebe o ID e os novos dados. Retorna o DTO atualizado (ou null se não achar).
public record UpdatePersonCommand(
    int Id,
    string Name,
    int Age,
    string Gender
) : IRequest<PersonDto?>;