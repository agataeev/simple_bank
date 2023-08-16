namespace AccountManagement.Models.Requests;

public class UpdateModel
{
    public long Id { get; set; }
    public string? Username { get; set; }
    public string? Pin { get; set; }
}