using MediatR;
using VaccinationCard.Application.DTOs;

namespace VaccinationCard.Application.UseCases.Persons.Commands.CreatePerson;

public record CreatePersonCommand(
    string Name,
    int Age,
    string Gender
) : IRequest<PersonDto>;