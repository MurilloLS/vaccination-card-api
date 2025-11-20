using VaccinationCard.Domain.Entities;

namespace VaccinationCard.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task AddAsync(User user);
}