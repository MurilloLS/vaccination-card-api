using VaccinationCard.Domain.Entities;

namespace VaccinationCard.Domain.Interfaces;

public interface IPersonRepository
{
    Task<Person?> GetByIdAsync(int id);
    Task<IEnumerable<Person>> GetAllAsync();
    Task<Person> AddAsync(Person person);
    Task UpdateAsync(Person person);
    Task DeleteAsync(Person person);
}