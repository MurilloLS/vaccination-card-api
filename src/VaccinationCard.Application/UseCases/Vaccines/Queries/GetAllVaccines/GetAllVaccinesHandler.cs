using AutoMapper;
using MediatR;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Domain.Interfaces;

namespace VaccinationCard.Application.UseCases.Vaccines.Queries.GetAllVaccines;

public class GetAllVaccinesHandler : IRequestHandler<GetAllVaccinesQuery, IEnumerable<VaccineDto>>
{
    private readonly IVaccineRepository _repository;
    private readonly IMapper _mapper;

    public GetAllVaccinesHandler(IVaccineRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VaccineDto>> Handle(GetAllVaccinesQuery request, CancellationToken cancellationToken)
    {
        var vaccines = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<VaccineDto>>(vaccines);
    }
}