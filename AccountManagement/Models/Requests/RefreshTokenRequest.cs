using System.ComponentModel.DataAnnotations;

namespace AccountManagement.Models.Requests;

public class RefreshTokenRequest
{
    [Required]
    public string? RefreshToken { get; set; }
}