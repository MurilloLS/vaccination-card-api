using System.Reflection;
using Microsoft.EntityFrameworkCore;
using VaccinationCard.Domain.Entities;

namespace VaccinationCard.Infrastructure.Persistence;

public class VaccinationDbContext : DbContext
{
    public VaccinationDbContext(DbContextOptions<VaccinationDbContext> options) : base(options)
    {
    }

    public DbSet<Person> Persons { get; set; }
    public DbSet<Vaccine> Vaccines { get; set; }
    public DbSet<VaccineCategory> VaccineCategories { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Vaccination> Vaccinations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aplica todas as configurações da pasta Configurations automaticamente
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
    }
}