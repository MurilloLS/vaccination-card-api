namespace VaccinationCard.Domain.Constants;

public static class DoseType
{
    public const string Dose1 = "D1";     // Tipo - 1 dose
    public const string Dose2 = "D2";     // Tipo - 2 dose
    public const string Dose3 = "D3";     // Tipo - 3 dose
    public const string Reforco1 = "R1";  // Tipo - 1 reforço
    public const string Reforco2 = "R2";  // Tipo - 2 reforço

    public static readonly HashSet<string> All = new() { Dose1, Dose2, Dose3, Reforco1, Reforco2 };
}