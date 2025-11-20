using VaccinationCard.Domain.Exceptions;

namespace VaccinationCard.Domain.Entities;

public sealed class VaccineCategory
{
    // id_vaccine_category
    public int Id { get; private set; }

    // nm_vaccine_category
    public string Name { get; private set; } = null!;

    public ICollection<Vaccine> Vaccines { get; private set; } = new List<Vaccine>();

    private VaccineCategory() { }

    public VaccineCategory(string name)
    {
        DomainException.When(string.IsNullOrEmpty(name), "Category name is required.");
        Name = name;
    }
}