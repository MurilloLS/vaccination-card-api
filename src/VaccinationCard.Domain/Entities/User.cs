using VaccinationCard.Domain.Exceptions;

namespace VaccinationCard.Domain.Entities;

public sealed class User
{
    // id_user
    public int Id { get; private set; }

    // nm_user
    public string Username { get; private set; } = null!;

    // pwd_hash_user
    public string PasswordHash { get; private set; } = null!;

    // sg_role (USER, ADMIN)
    public string Role { get; private set; } = null!;

    private User() { }

    public User(string username, string passwordHash, string role)
    {
        ValidateDomain(username, passwordHash, role);
        Username = username;
        PasswordHash = passwordHash;
        Role = role;
    }

    private void ValidateDomain(string username, string passwordHash, string role)
    {
        DomainException.When(string.IsNullOrEmpty(username), "Username is required.");
        DomainException.When(string.IsNullOrEmpty(passwordHash), "Password hash is required.");
        DomainException.When(string.IsNullOrEmpty(role), "Role is required.");
        
        DomainException.When(role.Length > 5, "Role cannot exceed 5 characters.");
    }
}