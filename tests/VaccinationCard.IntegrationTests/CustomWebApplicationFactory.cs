using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VaccinationCard.Application.Common.Interfaces;
using VaccinationCard.Infrastructure.Persistence;

namespace VaccinationCard.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // 1. Injeção de Configuração
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                {"Jwt:Secret", "UmaChaveMuitoSeguraEMuitoLongaParaOAlgoritmoHmacSha256FuncionarBem!!!"},
                {"Jwt:Issuer", "VaccinationApi"},
                {"Jwt:Audience", "VaccinationClient"}
            });
        });

        builder.ConfigureServices(services =>
        {
            // 2. Troca de Banco
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<VaccinationDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<VaccinationDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });

            // 3. Seed de Dados 
            var sp = services.BuildServiceProvider();
            
            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<VaccinationDbContext>();
                var hasher = scopedServices.GetRequiredService<IPasswordHasher>(); 

                db.Database.EnsureCreated();
                
                try
                {
                    DbInitializer.Initialize(db, hasher);
                }
                catch
                {
                    // Ignora erro se já existir
                }
            }
        });
    }
}