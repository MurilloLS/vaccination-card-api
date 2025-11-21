using VaccinationCard.Domain.Entities;
using VaccinationCard.Application.Common.Interfaces;

namespace VaccinationCard.Infrastructure.Persistence;

public static class DbInitializer
{
    // Adicione o parâmetro 'hasher'
    public static void Initialize(VaccinationDbContext context, IPasswordHasher hasher)
    {
        context.Database.EnsureCreated();

        SeedCategoriesAndVaccines(context);
        SeedUsers(context, hasher); // <--- Novo método
    }

    private static void SeedUsers(VaccinationDbContext context, IPasswordHasher hasher)
    {
        if (context.Users.Any()) return;

        // Cria um Admin padrão
        var admin = new User(
            "admin", 
            hasher.Hash("admin123"), // Senha forte ;)
            "ADMIN" // <--- A Role mágica
        );

        context.Users.Add(admin);
        context.SaveChanges();
    }

    private static void SeedCategoriesAndVaccines(VaccinationDbContext context)
    {
        if (context.VaccineCategories.Any()) return;

        var catNacional = new VaccineCategory("Carteira Nacional de Vacinação");

        context.VaccineCategories.Add(catNacional);
        context.SaveChanges(); 

        // Se no futuro quisermos movê-las para uma aba "Particular", é só rodar um UPDATE no banco.
        var vacinas = new List<Vaccine>
        {
            new Vaccine("BCG", catNacional.Id),
            new Vaccine("HEPATITE B", catNacional.Id),
            new Vaccine("ANTI-POLIO (SABIN)", catNacional.Id),
            new Vaccine("TETRA VALENTE", catNacional.Id),
            new Vaccine("TRIPLICE BACTERIANA (DPT)", catNacional.Id),
            new Vaccine("HAEMOPHILUS INFLUENZAE", catNacional.Id),
            new Vaccine("PNEUMO 10 VALENTE", catNacional.Id),
            new Vaccine("ROTAVIRUS", catNacional.Id),            
            new Vaccine("TRIPLICE ACELULAR", catNacional.Id),
            new Vaccine("MENINGO B", catNacional.Id)
        };

        context.Vaccines.AddRange(vacinas);
        context.SaveChanges();
    }
}