using AutoMapper;
using MediatR;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Domain.Entities;
using VaccinationCard.Domain.Interfaces;

namespace VaccinationCard.Application.UseCases.Persons.Commands.CreatePerson;

public class CreatePersonHandler : IRequestHandler<CreatePersonCommand, PersonDto>
{
    private readonly IPersonRepository _repository;
    private readonly IMapper _mapper;

    public CreatePersonHandler(IPersonRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PersonDto> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = new Person(request.Name, request.Age, request.Gender);
        await _repository.AddAsync(person);
        
        return _mapper.Map<PersonDto>(person);
    }
}