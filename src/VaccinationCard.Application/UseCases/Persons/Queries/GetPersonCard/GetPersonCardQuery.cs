using MediatR;
using VaccinationCard.Application.DTOs;

namespace VaccinationCard.Application.UseCases.Persons.Queries.GetPersonCard;

public record GetPersonCardQuery(int Id) : IRequest<PersonCardDto?>;