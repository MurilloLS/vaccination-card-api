using VaccinationCard.Domain.Entities;

namespace VaccinationCard.Application.Common.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}