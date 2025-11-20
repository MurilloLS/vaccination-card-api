using Microsoft.EntityFrameworkCore;
using VaccinationCard.Domain.Entities;
using VaccinationCard.Domain.Interfaces;
using VaccinationCard.Infrastructure.Persistence;

namespace VaccinationCard.Infrastructure.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly VaccinationDbContext _context;

    public PersonRepository(VaccinationDbContext context)
    {
        _context = context;
    }

    public async Task<Person?> GetByIdAsync(int id)
    {
        // Inclui as vacinações ao buscar a pessoa (Eager Loading)
        return await _context.Persons
            .Include(p => p.Vaccinations)
            .ThenInclude(v => v.Vaccine)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Person>> GetAllAsync()
    {
        return await _context.Persons.ToListAsync();
    }

    public async Task<Person> AddAsync(Person person)
    {
        _context.Persons.Add(person);
        await _context.SaveChangesAsync();
        return person;
    }

    public async Task UpdateAsync(Person person)
    {
        _context.Persons.Update(person);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Person person)
    {
        _context.Persons.Remove(person);
        await _context.SaveChangesAsync();
    }
}