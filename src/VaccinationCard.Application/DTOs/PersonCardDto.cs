namespace VaccinationCard.Application.DTOs;

public class PersonCardDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Gender { get; set; } = string.Empty;

    public ICollection<VaccinationDto> Vaccinations { get; set; } = new List<VaccinationDto>();
}