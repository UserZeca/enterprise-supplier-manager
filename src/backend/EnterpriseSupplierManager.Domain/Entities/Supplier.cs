namespace EnterpriseSupplierManager.Domain.Entities;

public class Supplier
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty; // CPF ou CNPJ
    public string Email { get; set; } = string.Empty;
    public string Cep { get; set; } = string.Empty;

    public string? Rg { get; set; }
    public DateTime? BirthDate { get; set; }

    public ICollection<Company> Companies { get; set; } = new List<Company>();

    // Helper to check if someone is underage
    public bool IsMinor()
    {
        if (!BirthDate.HasValue) return false;
        var age = DateTime.Today.Year - BirthDate.Value.Year;
        if (BirthDate.Value.Date > DateTime.Today.AddYears(-age)) age--;
        return age < 18;
    }
}