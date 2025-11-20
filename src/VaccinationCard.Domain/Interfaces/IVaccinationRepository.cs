using VaccinationCard.Domain.Entities;

namespace VaccinationCard.Domain.Interfaces;

public interface IVaccinationRepository
{
    Task<Vaccination> AddAsync(Vaccination vaccination);
    Task<IEnumerable<Vaccination>> GetByPersonIdAsync(int personId);
    Task<Vaccination?> GetByIdAsync(int id);
    Task DeleteAsync(Vaccination vaccination);
}