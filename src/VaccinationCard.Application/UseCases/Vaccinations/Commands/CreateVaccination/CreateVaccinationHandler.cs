using AutoMapper;
using MediatR;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Domain.Entities;
using VaccinationCard.Domain.Exceptions; 
using VaccinationCard.Domain.Interfaces;

namespace VaccinationCard.Application.UseCases.Vaccinations.Commands.CreateVaccination;

public class CreateVaccinationHandler : IRequestHandler<CreateVaccinationCommand, VaccinationDto>
{
    private readonly IVaccinationRepository _vaccinationRepo;
    private readonly IPersonRepository _personRepo;
    private readonly IVaccineRepository _vaccineRepo;
    private readonly IMapper _mapper;

    public CreateVaccinationHandler(
        IVaccinationRepository vaccinationRepo,
        IPersonRepository personRepo,
        IVaccineRepository vaccineRepo,
        IMapper mapper)
    {
        _vaccinationRepo = vaccinationRepo;
        _personRepo = personRepo;
        _vaccineRepo = vaccineRepo;
        _mapper = mapper;
    }

    public async Task<VaccinationDto> Handle(CreateVaccinationCommand request, CancellationToken cancellationToken)
    {
        // 1. Validar existência da Pessoa
        var person = await _personRepo.GetByIdAsync(request.PersonId);
        DomainException.When(person == null, "Person not found.");

        // 2. Validar existência da Vacina
        var vaccine = await _vaccineRepo.GetByIdAsync(request.VaccineId);
        DomainException.When(vaccine == null, "Vaccine not found.");

        // 3. Criar Entidade
        var vaccination = new Vaccination(
            request.PersonId,
            request.VaccineId,
            request.Dose,
            request.ApplicationDate
        );

        // 4. Persistir
        await _vaccinationRepo.AddAsync(vaccination);

        // 5. Mapear para DTO
        var dto = _mapper.Map<VaccinationDto>(vaccination);
        dto.PersonName = person!.Name;
        dto.VaccineName = vaccine!.Name;

        return dto;
    }
}