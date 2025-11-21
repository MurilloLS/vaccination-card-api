using MediatR;
using VaccinationCard.Application.DTOs;

namespace VaccinationCard.Application.UseCases.Persons.Commands.DeletePerson;
public record DeletePersonCommand(int Id) : IRequest<PersonDto?>;