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
    public int MaxDoses { get; private set; }
    private Vaccine() { }

    public Vaccine(string name, int categoryId, int maxDoses)
    {
        ValidateDomain(name, categoryId, maxDoses);
        Name = name;
        CategoryId = categoryId;
        MaxDoses = maxDoses;
    }

    public void Update(string name, int categoryId, int maxDoses)
    {
        ValidateDomain(name, categoryId, maxDoses);
        Name = name;
        CategoryId = categoryId;
        MaxDoses = maxDoses;
    }

    private void ValidateDomain(string name, int categoryId, int maxDoses)
    {
        DomainException.When(string.IsNullOrEmpty(name), "Vaccine name is required.");
        DomainException.When(categoryId <= 0, "Invalid Category ID.");
        DomainException.When(maxDoses <= 0 || maxDoses > 5, "Max Doses must be between 1 and 5.");
    }
}