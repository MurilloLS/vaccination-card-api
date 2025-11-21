namespace VaccinationCard.Application.DTOs;

public class VaccinationDto
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public int VaccineId { get; set; }
    public string VaccineName { get; set; } = string.Empty; 
    public string Dose { get; set; } = string.Empty;
    public DateTime ApplicationDate { get; set; }
}