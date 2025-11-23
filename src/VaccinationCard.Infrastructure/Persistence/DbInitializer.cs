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
                new VaccineCategory("Carteina Nacional de Vacinação"), 
                new VaccineCategory("Anti Rabica"),
                new VaccineCategory("BCG de Contato"),
                new VaccineCategory("Vacinas Particulares"),
                new VaccineCategory("Outras Vacinas")
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
                    // 1 Dose
                    new Vaccine("BCG", catNacional.Id, 1),
                    new Vaccine("FEBRE AMARELA", catNacional.Id, 1),
                    new Vaccine("ROTAVIRUS", catNacional.Id, 2),

                    // 3 Doses
                    new Vaccine("HEPATITE B", catNacional.Id, 3),
                    new Vaccine("PNEUMO 10 VALENTE", catNacional.Id, 3),
                    new Vaccine("MENINGO B", catNacional.Id, 3),

                    // 5 Doses (Esquema Completo D1..R2)
                    new Vaccine("ANTI-POLIO (SABIN)", catNacional.Id, 5),
                    new Vaccine("TETRA VALENTE", catNacional.Id, 5),
                    new Vaccine("TRIPLICE BACTERIANA (DPT)", catNacional.Id, 5),
                    new Vaccine("HAEMOPHILUS INFLUENZAE", catNacional.Id, 3),
                    new Vaccine("TRIPLICE ACELULAR", catNacional.Id, 3)
                };

                context.Vaccines.AddRange(vacinas);
                context.SaveChanges();
            }
        }
    }
}