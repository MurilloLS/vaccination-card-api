using VaccinationCard.Domain.Exceptions;

namespace VaccinationCard.Domain.Entities;

public sealed class Vaccination
{
    // id_vaccination
    public int Id { get; private set; }

    // FKs
    public int PersonId { get; private set; }
    public Person Person { get; private set; } = null!;

    public int VaccineId { get; private set; }
    public Vaccine Vaccine { get; private set; } = null!;

    // cd_dose_vaccination (CHAR 5)
    public string Dose { get; private set; } = null!;

    // dt_application_vaccination
    public DateTime ApplicationDate { get; private set; }

    private Vaccination() { }

    public Vaccination(int personId, int vaccineId, string dose, DateTime applicationDate)
    {
        ValidateDomain(personId, vaccineId, dose, applicationDate);
        PersonId = personId;
        VaccineId = vaccineId;
        Dose = dose;
        ApplicationDate = applicationDate;
    }

    private void ValidateDomain(int personId, int vaccineId, string dose, DateTime applicationDate)
    {
        DomainException.When(personId <= 0, "Invalid Person ID.");
        DomainException.When(vaccineId <= 0, "Invalid Vaccine ID.");
        DomainException.When(string.IsNullOrEmpty(dose), "Dose is required.");
        DomainException.When(dose.Length > 5, "Dose code cannot exceed 5 characters.");
        DomainException.When(applicationDate > DateTime.Now, "Application date cannot be in the future.");
    }

    public void Update(int vaccineId, string dose, DateTime applicationDate)
    {
        ValidateDomain(PersonId, vaccineId, dose, applicationDate);
        VaccineId = vaccineId;
        Dose = dose;
        ApplicationDate = applicationDate;
    }
}