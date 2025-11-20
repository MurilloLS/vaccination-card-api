using VaccinationCard.Domain.Exceptions;

namespace VaccinationCard.Domain.Entities;

public sealed class Vaccine
{
    // id_vaccine
    public int Id { get; private set; }

    // nm_vaccine
    public string Name { get; private set; } = null!;

    // id_vaccine_category (FK)
    public int CategoryId { get; private set; }
    public VaccineCategory Category { get; private set; } = null!;
    private Vaccine() { }

    public Vaccine(string name, int categoryId)
    {
        DomainException.When(string.IsNullOrEmpty(name), "Vaccine name is required.");
        DomainException.When(categoryId <= 0, "Invalid Category ID.");
        Name = name;
        CategoryId = categoryId;
    }
}