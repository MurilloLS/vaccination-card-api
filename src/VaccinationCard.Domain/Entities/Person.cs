using VaccinationCard.Domain.Exceptions;

namespace VaccinationCard.Domain.Entities;

public sealed class Person
{
    // id_person
    public int Id { get; private set; }

    // nm_person
    public string Name { get; private set; } = null!;

    // nr_age_person
    public int Age { get; private set; }

    // sg_gender_person
    public string Gender { get; private set; } = null!;

    public ICollection<Vaccination> Vaccinations { get; set; } = new List<Vaccination>();

    private Person() { }

    public Person(string name, int age, string gender)
    {
        ValidateDomain(name, age, gender);
        Name = name;
        Age = age;
        Gender = gender;
    }

    public void Update(string name, int age, string gender)
    {
        ValidateDomain(name, age, gender);
        Name = name;
        Age = age;
        Gender = gender;
    }

    private void ValidateDomain(string name, int age, string gender)
    {
        DomainException.When(string.IsNullOrEmpty(name), "Name is required.");
        DomainException.When(name.Length > 150, "Name cannot exceed 150 characters.");
        
        DomainException.When(age < 0, "Age cannot be negative.");
        
        DomainException.When(string.IsNullOrEmpty(gender), "Gender is required.");
        DomainException.When(gender.Length > 1, "Gender must be 1 character.");
    }
}