using VaccinationCard.Domain.Entities;

namespace VaccinationCard.Domain.Interfaces;

public interface IVaccineRepository
{
    Task<Vaccine?> GetByIdAsync(int id);
    Task<IEnumerable<Vaccine>> GetAllAsync();
    Task<Vaccine> AddAsync(Vaccine vaccine);

    Task UpdateAsync(Vaccine vaccine);
    Task DeleteAsync(Vaccine vaccine);

    Task<bool> HasVaccinationAsync(int vaccineId);
}