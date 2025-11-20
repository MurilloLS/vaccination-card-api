using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VaccinationCard.Domain.Entities;

namespace VaccinationCard.Infrastructure.Persistence.Configurations;

public class VaccineCategoryConfiguration : IEntityTypeConfiguration<VaccineCategory>
{
    public void Configure(EntityTypeBuilder<VaccineCategory> builder)
    {
        builder.ToTable("VACCINE_CATEGORY");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id_vaccine_category");

        builder.Property(c => c.Name)
            .HasColumnName("nm_vaccine_category")
            .HasMaxLength(100)
            .IsRequired();
    }
}