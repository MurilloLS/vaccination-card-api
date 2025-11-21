using AutoMapper;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Domain.Entities;

namespace VaccinationCard.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Person, PersonDto>();
        CreateMap<Person, PersonCardDto>();
    }
}