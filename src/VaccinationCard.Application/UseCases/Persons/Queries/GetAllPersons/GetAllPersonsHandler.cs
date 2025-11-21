using AutoMapper;
using MediatR;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Domain.Interfaces;

namespace VaccinationCard.Application.UseCases.Persons.Queries.GetAllPersons;

public class GetAllPersonsHandler : IRequestHandler<GetAllPersonsQuery, IEnumerable<PersonDto>>
{
    private readonly IPersonRepository _repository;
    private readonly IMapper _mapper;

    public GetAllPersonsHandler(IPersonRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PersonDto>> Handle(GetAllPersonsQuery request, CancellationToken cancellationToken)
    {
        var persons = await _repository.GetAllAsync();

        // Mapeia lista de Entities para lista de DTOs
        return _mapper.Map<IEnumerable<PersonDto>>(persons);
    }
}