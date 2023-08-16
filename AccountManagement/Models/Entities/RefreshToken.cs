namespace AccountManagement.Models.Entities;

public class RefreshToken: Base
{
    public string? Value { get; set; }
    public DateTime? Created { get; set; }
    public bool Revoked { get; set; }
    public long UserId { get; set; }
    public string? AccessToken { get; set; }
}