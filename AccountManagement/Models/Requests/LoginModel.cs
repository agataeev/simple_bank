using System.ComponentModel.DataAnnotations;

namespace AccountManagement.Models.Requests;

public class LoginModel
{
    [Required]
    public string? Username { get; set; }
    [Required]
    public string? Pin { get; set; }
}