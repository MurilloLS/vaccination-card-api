using AutoMapper;
using MediatR;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Domain.Interfaces;

namespace VaccinationCard.Application.UseCases.Vaccinations.Commands.DeleteVaccination;

public class DeleteVaccinationHandler : IRequestHandler<DeleteVaccinationCommand, VaccinationDto?>
{
    private readonly IVaccinationRepository _repository;
    private readonly IMapper _mapper;

    public DeleteVaccinationHandler(IVaccinationRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<VaccinationDto?> Handle(DeleteVaccinationCommand request, CancellationToken cancellationToken)
    {
        var vaccination = await _repository.GetByIdAsync(request.Id);
        if (vaccination == null) return null;

        var dto = _mapper.Map<VaccinationDto>(vaccination);
        
        await _repository.DeleteAsync(vaccination);
        
        return dto;
    }
}