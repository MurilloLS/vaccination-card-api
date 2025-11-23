using VaccinationCard.Domain.Entities;
using VaccinationCard.Application.Common.Interfaces;

namespace VaccinationCard.Infrastructure.Persistence
{
    public static class DbInitializer
    {
        public static void Initialize(VaccinationDbContext context, IPasswordHasher hasher)
        {
            context.Database.EnsureCreated();

            SeedCategories(context);
            SeedUsers(context, hasher);
            SeedVaccines(context);
        }

        private static void SeedCategories(VaccinationDbContext context)
        {
            if (context.VaccineCategories.Any()) return;

            var categories = new List<VaccineCategory>
            {
                new VaccineCategory("Carteina Nacional de Vacinação"), // ID 1
                new VaccineCategory("Anti Rabica"),                    // ID 2
                new VaccineCategory("BCG de Contato"),                 // ID 3
                new VaccineCategory("Vacinas Particulares"),           // ID 4
                new VaccineCategory("Outras Vacinas")                  // ID 5
            };

            context.VaccineCategories.AddRange(categories);
            context.SaveChanges();
        }

        private static void SeedUsers(VaccinationDbContext context, IPasswordHasher hasher)
        {
            if (context.Users.Any()) return;

            var admin = new User(
                "admin", 
                hasher.Hash("admin123"), 
                "ADMIN"
            );

            context.Users.Add(admin);
            context.SaveChanges();
        }

        private static void SeedVaccines(VaccinationDbContext context)
        {
            if (context.Vaccines.Any()) return;
            
            var catNacional = context.VaccineCategories
                .FirstOrDefault(c => c.Name == "Carteina Nacional de Vacinação");

            if (catNacional != null)
            {
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
    }
}