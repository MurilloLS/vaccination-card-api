using Microsoft.EntityFrameworkCore;
using VaccinationCard.Domain.Entities;
using VaccinationCard.Domain.Interfaces;
using VaccinationCard.Infrastructure.Persistence;

namespace VaccinationCard.Infrastructure.Repositories;

public class VaccinationRepository : IVaccinationRepository
{
    private readonly VaccinationDbContext _context;

    public VaccinationRepository(VaccinationDbContext context)
    {
        _context = context;
    }

    public async Task<Vaccination> AddAsync(Vaccination vaccination)
    {
        _context.Vaccinations.Add(vaccination);
        await _context.SaveChangesAsync();
        return vaccination;
    }

    public async Task<IEnumerable<Vaccination>> GetByPersonIdAsync(int personId)
    {
        return await _context.Vaccinations
            .Where(v => v.PersonId == personId)
            .Include(v => v.Vaccine)
            .OrderBy(v => v.ApplicationDate)
            .ToListAsync();
    }

    public async Task<Vaccination?> GetByIdAsync(int id)
    {
        return await _context.Vaccinations.FindAsync(id);
    }

    public async Task DeleteAsync(Vaccination vaccination)
    {
        _context.Vaccinations.Remove(vaccination);
        await _context.SaveChangesAsync();
    }
}