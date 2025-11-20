using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VaccinationCard.Domain.Entities;

namespace VaccinationCard.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("USER");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnName("id_user");

        builder.Property(u => u.Username)
            .HasColumnName("nm_user")
            .HasMaxLength(100)
            .IsRequired();
        
        // Unique constraint
        builder.HasIndex(u => u.Username).IsUnique();

        builder.Property(u => u.PasswordHash)
            .HasColumnName("pwd_hash_user")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.Role)
            .HasColumnName("sg_role")
            .HasColumnType("CHAR(5)")
            .HasDefaultValue("USER")
            .IsRequired();
    }
}