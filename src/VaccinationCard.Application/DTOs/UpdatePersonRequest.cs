namespace VaccinationCard.Application.DTOs;

// Este objeto NÃO tem ID, pois o ID vem da URL
public record UpdatePersonRequest(
    string Name,
    int Age,
    string Gender
);