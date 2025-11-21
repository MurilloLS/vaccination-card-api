using AutoMapper; 
using MediatR;
using VaccinationCard.Application.DTOs; 
using VaccinationCard.Domain.Interfaces;

namespace VaccinationCard.Application.UseCases.Persons.Commands.DeletePerson;

public class DeletePersonHandler : IRequestHandler<DeletePersonCommand, PersonDto?>
{
    private readonly IPersonRepository _repository;
    private readonly IMapper _mapper; 

    public DeletePersonHandler(IPersonRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PersonDto?> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await _repository.GetByIdAsync(request.Id);

        if (person == null) return null;

        // Salva uma cópia dos dados no DTO (Memória)
        var personDto = _mapper.Map<PersonDto>(person);

        await _repository.DeleteAsync(person);

        return personDto;
    }
}