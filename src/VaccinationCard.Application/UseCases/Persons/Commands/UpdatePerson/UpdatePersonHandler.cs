using AutoMapper;
using MediatR;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Domain.Interfaces;

namespace VaccinationCard.Application.UseCases.Persons.Commands.UpdatePerson;

public class UpdatePersonHandler : IRequestHandler<UpdatePersonCommand, PersonDto?>
{
    private readonly IPersonRepository _repository;
    private readonly IMapper _mapper;

    public UpdatePersonHandler(IPersonRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PersonDto?> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await _repository.GetByIdAsync(request.Id);

        if (person == null) return null;

        // Isso garante que as validações internas da entidade rodem (ex: idade negativa)
        person.Update(request.Name, request.Age, request.Gender);

        await _repository.UpdateAsync(person);
        
        return _mapper.Map<PersonDto>(person);
    }
}