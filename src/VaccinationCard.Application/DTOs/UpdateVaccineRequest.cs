namespace VaccinationCard.Application.DTOs;
public record UpdateVaccineRequest(
    string Name, 
    int CategoryId
);