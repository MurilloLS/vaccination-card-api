using Microsoft.EntityFrameworkCore;
using VaccinationCard.Domain.Entities;
using VaccinationCard.Domain.Interfaces;
using VaccinationCard.Infrastructure.Persistence;

namespace VaccinationCard.Infrastructure.Repositories;

public class VaccineRepository : IVaccineRepository
{
    private readonly VaccinationDbContext _context;

    public VaccineRepository(VaccinationDbContext context)
    {
        _context = context;
    }

    public async Task<Vaccine?> GetByIdAsync(int id)
    {
        return await _context.Vaccines
            .Include(v => v.Category)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<IEnumerable<Vaccine>> GetAllAsync()
    {
        return await _context.Vaccines
            .Include(v => v.Category)
            .ToListAsync();
    }

    public async Task<Vaccine> AddAsync(Vaccine vaccine)
    {
        _context.Vaccines.Add(vaccine);
        await _context.SaveChangesAsync();
        return vaccine;
    }

    public async Task UpdateAsync(Vaccine vaccine)
    {
        _context.Vaccines.Update(vaccine);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Vaccine vaccine)
    {
        _context.Vaccines.Remove(vaccine);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> HasVaccinationAsync(int vaccineId)
    {
        return await _context.Vaccinations.AnyAsync(v => v.VaccineId == vaccineId);
    }
}