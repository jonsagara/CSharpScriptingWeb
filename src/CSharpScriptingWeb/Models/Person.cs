namespace CSharpScriptingWeb.Models;

public record Person
{
    public string LastName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string MiddleName { get; set; } = null!;
    public string BirthDate { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string Zip { get; set; } = null!;
    public string County { get; set; } = null!;
    public string Social { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public int Pounds { get; set; }
    public int Feet { get; set; }
    public int Inches { get; set; }
    public string BirthCity { get; set; } = null!;
    public string BirthState { get; set; } = null!;
    public string Sex { get; set; } = null!;
    public string? CountryCode { get; set; }

    public string FullName => $"{FirstName} {MiddleName} {LastName}";
}
