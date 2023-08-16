namespace AccountManagement.Models.Entities;

public class User: Base
{
    public string? Username { get; set; }
    public string? Pin { get; set; }
    public int Rights { get; set; }
}