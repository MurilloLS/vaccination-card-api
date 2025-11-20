using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VaccinationCard.Domain.Entities;

namespace VaccinationCard.Infrastructure.Persistence.Configurations;

public class VaccinationConfiguration : IEntityTypeConfiguration<Vaccination>
{
    public void Configure(EntityTypeBuilder<Vaccination> builder)
    {
        builder.ToTable("VACCINATION");

        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id).HasColumnName("id_vaccination");

        builder.Property(v => v.PersonId).HasColumnName("id_person");
        builder.Property(v => v.VaccineId).HasColumnName("id_vaccine");

        builder.Property(v => v.Dose)
            .HasColumnName("cd_dose_vaccination")
            .HasColumnType("CHAR(5)")
            .IsRequired();

        builder.Property(v => v.ApplicationDate)
            .HasColumnName("dt_application_vaccination")
            .HasColumnType("DATE")
            .IsRequired();

        // Relacionamentos (FKs)
        builder.HasOne(v => v.Person)
            .WithMany(p => p.Vaccinations)
            .HasForeignKey(v => v.PersonId);

        builder.HasOne(v => v.Vaccine)
            .WithMany()
            .HasForeignKey(v => v.VaccineId);
    }
}