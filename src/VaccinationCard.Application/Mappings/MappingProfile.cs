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

        CreateMap<Vaccination, VaccinationDto>()
        .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.Person.Name))
        .ForMember(dest => dest.VaccineName, opt => opt.MapFrom(src => src.Vaccine.Name));
    }
}