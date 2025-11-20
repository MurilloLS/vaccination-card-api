using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VaccinationCard.Domain.Entities;

namespace VaccinationCard.Infrastructure.Persistence.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("PERSON");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id_person");

        builder.Property(p => p.Name)
            .HasColumnName("nm_person")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(p => p.Age)
            .HasColumnName("nr_age_person")
            .IsRequired();

        builder.Property(p => p.Gender)
            .HasColumnName("sg_gender_person")
            .HasColumnType("CHAR(1)")
            .IsRequired();

        // Relacionamento: Se deletar Pessoa, deleta as Vacinações (Cascade)
        builder.HasMany(p => p.Vaccinations)
            .WithOne(v => v.Person)
            .HasForeignKey(v => v.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}