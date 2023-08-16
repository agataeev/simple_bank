namespace AccountManagement.Models.Requests;

public class CreateAccountRequest
{
    public int Type { get; set; }
    public decimal Balance { get; set; }
    public int Currency { get; set; }
    public bool Active { get; set; }
}