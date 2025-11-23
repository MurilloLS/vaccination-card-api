using AutoMapper;
using MediatR;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Domain.Entities;
using VaccinationCard.Domain.Interfaces;

namespace VaccinationCard.Application.UseCases.Vaccines.Commands.CreateVaccine;

public class CreateVaccineHandler : IRequestHandler<CreateVaccineCommand, VaccineDto>
{
    private readonly IVaccineRepository _vaccineRepository;
    private readonly IMapper _mapper;

    public CreateVaccineHandler(IVaccineRepository vaccineRepository, IMapper mapper)
    {
        _vaccineRepository = vaccineRepository;
        _mapper = mapper;
    }

    public async Task<VaccineDto> Handle(CreateVaccineCommand request, CancellationToken cancellationToken)
    {
        var vaccine = new Vaccine(request.Name, request.CategoryId, request.MaxDoses);

        await _vaccineRepository.AddAsync(vaccine);
        var vaccineCompleta = await _vaccineRepository.GetByIdAsync(vaccine.Id);

        return _mapper.Map<VaccineDto>(vaccineCompleta);
    }
}