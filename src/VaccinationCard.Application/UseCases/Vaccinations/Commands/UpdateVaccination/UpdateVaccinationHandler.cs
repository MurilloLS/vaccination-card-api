using AutoMapper;
using MediatR;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Domain.Exceptions;
using VaccinationCard.Domain.Interfaces;

namespace VaccinationCard.Application.UseCases.Vaccinations.Commands.UpdateVaccination;

public class UpdateVaccinationHandler : IRequestHandler<UpdateVaccinationCommand, VaccinationDto?>
{
    private readonly IVaccinationRepository _vaccinationRepo;
    private readonly IVaccineRepository _vaccineRepo;
    private readonly IMapper _mapper;

    public UpdateVaccinationHandler(
        IVaccinationRepository vaccinationRepo,
        IVaccineRepository vaccineRepo, 
        IMapper mapper)
    {
        _vaccinationRepo = vaccinationRepo;
        _vaccineRepo = vaccineRepo;
        _mapper = mapper;
    }

    public async Task<VaccinationDto?> Handle(UpdateVaccinationCommand request, CancellationToken cancellationToken)
    {
        var vaccination = await _vaccinationRepo.GetByIdAsync(request.Id);
        if (vaccination == null) return null;

        // Verifica se a nova vacina existe
        var vaccine = await _vaccineRepo.GetByIdAsync(request.VaccineId);
        DomainException.When(vaccine == null, "Vaccine not found.");

        // Atualiza a entidade
        vaccination.Update(request.VaccineId, request.Dose, request.ApplicationDate);

        // Persiste
        await _vaccinationRepo.UpdateAsync(vaccination);

        return _mapper.Map<VaccinationDto>(vaccination);
    }
}