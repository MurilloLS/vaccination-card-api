using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VaccinationCard.Domain.Entities;

namespace VaccinationCard.Infrastructure.Persistence.Configurations;

public class VaccineConfiguration : IEntityTypeConfiguration<Vaccine>
{
    public void Configure(EntityTypeBuilder<Vaccine> builder)
    {
        builder.ToTable("VACCINE");

        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id).HasColumnName("id_vaccine");

        builder.Property(v => v.Name)
            .HasColumnName("nm_vaccine")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(v => v.CategoryId).HasColumnName("id_vaccine_category");

        builder.HasOne(v => v.Category)
            .WithMany(c => c.Vaccines)
            .HasForeignKey(v => v.CategoryId);
    }
}