namespace BitirmeProjem.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string PreferredLanguage { get; set; } = "tr";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public FinancialProfile? FinancialProfile { get; set; }
}
