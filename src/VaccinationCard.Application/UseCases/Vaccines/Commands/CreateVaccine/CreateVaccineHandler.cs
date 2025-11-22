using AutoMapper;
using MediatR;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Domain.Entities;
using VaccinationCard.Domain.Interfaces;

namespace VaccinationCard.Application.UseCases.Vaccines.Commands.CreateVaccine;

public class CreateVaccineHandler : IRequestHandler<CreateVaccineCommand, VaccineDto>
{
    private readonly IVaccineRepository _repository;
    private readonly IMapper _mapper;

    public CreateVaccineHandler(IVaccineRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<VaccineDto> Handle(CreateVaccineCommand request, CancellationToken cancellationToken)
    {
        // Cria a entidade
        var vaccine = new Vaccine(request.Name, request.CategoryId);

        // Salva (O repositório já existe)
        await _repository.AddAsync(vaccine);

        // Retorna DTO (O Mapper já existe)
        return _mapper.Map<VaccineDto>(vaccine);
    }
}