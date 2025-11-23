namespace VaccinationCard.Application.DTOs;

public class CreateVaccineRequest
{
    public string Name { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public int MaxDoses { get; set; } // Novo campo
}