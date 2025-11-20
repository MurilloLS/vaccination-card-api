using VaccinationCard.Domain.Entities;

namespace VaccinationCard.Domain.Interfaces;

public interface IVaccineRepository
{
    Task<Vaccine?> GetByIdAsync(int id);
    Task<IEnumerable<Vaccine>> GetAllAsync();
    Task<Vaccine> AddAsync(Vaccine vaccine);
}