namespace VaccinationCard.Application.DTOs;

public record UpdateVaccinationRequest(
    int VaccineId,
    string Dose,
    DateTime ApplicationDate
);