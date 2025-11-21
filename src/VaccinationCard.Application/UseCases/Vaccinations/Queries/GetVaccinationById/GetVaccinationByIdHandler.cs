using AutoMapper;
using MediatR;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Domain.Interfaces;

namespace VaccinationCard.Application.UseCases.Vaccinations.Queries.GetVaccinationById;

public class GetVaccinationByIdHandler : IRequestHandler<GetVaccinationByIdQuery, VaccinationDto?>
{
    private readonly IVaccinationRepository _repository;
    private readonly IMapper _mapper;

    public GetVaccinationByIdHandler(IVaccinationRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<VaccinationDto?> Handle(GetVaccinationByIdQuery request, CancellationToken cancellationToken)
    {
        var vaccination = await _repository.GetByIdAsync(request.Id);
        return _mapper.Map<VaccinationDto>(vaccination);
    }
}