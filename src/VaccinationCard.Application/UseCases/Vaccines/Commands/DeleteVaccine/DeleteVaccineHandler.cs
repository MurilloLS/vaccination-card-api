using AutoMapper;
using MediatR;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Domain.Exceptions;
using VaccinationCard.Domain.Interfaces;

namespace VaccinationCard.Application.UseCases.Vaccines.Commands.DeleteVaccine;

public class DeleteVaccineHandler : IRequestHandler<DeleteVaccineCommand, VaccineDto?>
{
    private readonly IVaccineRepository _repository;
    private readonly IMapper _mapper;

    public DeleteVaccineHandler(IVaccineRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<VaccineDto?> Handle(DeleteVaccineCommand request, CancellationToken cancellationToken)
    {
        var vaccine = await _repository.GetByIdAsync(request.Id);
        if (vaccine == null) return null;

        var isInUse = await _repository.HasVaccinationAsync(vaccine.Id);
        if (isInUse)
        {
            // Lança erro 400 (via GlobalExceptionHandler)
            throw new DomainException("Cannot delete this vaccine because it has already been applied to patients.");
        }

        await _repository.DeleteAsync(vaccine);

        return _mapper.Map<VaccineDto>(vaccine);
    }
}