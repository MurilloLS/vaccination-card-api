using MediatR;
using VaccinationCard.Application.DTOs;

namespace VaccinationCard.Application.UseCases.Persons.Queries.GetAllPersons;

public record GetAllPersonsQuery() : IRequest<IEnumerable<PersonDto>>;