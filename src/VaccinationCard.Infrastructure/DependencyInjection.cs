using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VaccinationCard.Domain.Interfaces;
using VaccinationCard.Infrastructure.Persistence;
using VaccinationCard.Infrastructure.Repositories;

namespace VaccinationCard.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Configura o SQLite
        services.AddDbContext<VaccinationDbContext>(options =>
            options.UseSqlite("Data Source=vaccination_card.db"));

        // Injeta os Repositórios
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IVaccineRepository, VaccineRepository>();
        services.AddScoped<IVaccinationRepository, VaccinationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}