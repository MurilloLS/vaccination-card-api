using VaccinationCard.Domain.Entities;

namespace VaccinationCard.Infrastructure.Persistence;

public static class DbInitializer
{
    public static void Initialize(VaccinationDbContext context)
    {
        context.Database.EnsureCreated();

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