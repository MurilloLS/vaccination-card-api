using AutoMapper;
using MediatR;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Domain.Interfaces;

namespace VaccinationCard.Application.UseCases.Vaccines.Commands.UpdateVaccine;

public class UpdateVaccineHandler : IRequestHandler<UpdateVaccineCommand, VaccineDto?>
{
    private readonly IVaccineRepository _repository;
    private readonly IMapper _mapper;

    public UpdateVaccineHandler(IVaccineRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<VaccineDto?> Handle(UpdateVaccineCommand request, CancellationToken cancellationToken)
    {
        var vaccine = await _repository.GetByIdAsync(request.Id);
        if (vaccine == null) return null;

        vaccine.Update(request.Name, request.CategoryId);

        await _repository.UpdateAsync(vaccine);

        return _mapper.Map<VaccineDto>(vaccine);
    }
}