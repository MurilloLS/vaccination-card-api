using AutoMapper;
using MediatR;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Domain.Interfaces;

namespace VaccinationCard.Application.UseCases.Persons.Queries.GetPersonCard;

public class GetPersonCardHandler : IRequestHandler<GetPersonCardQuery, PersonCardDto?>
{
    private readonly IPersonRepository _repository;
    private readonly IMapper _mapper;

    public GetPersonCardHandler(IPersonRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PersonCardDto?> Handle(GetPersonCardQuery request, CancellationToken cancellationToken)
    {
        var person = await _repository.GetByIdAsync(request.Id);
        if (person == null) return null;

        // Mapeia Entity para DTO
        return _mapper.Map<PersonCardDto>(person);
    }
}