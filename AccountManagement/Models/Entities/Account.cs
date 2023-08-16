namespace AccountManagement.Models.Entities;

public class Account: Base
{
    public int Type { get; set; }
    public decimal Balance { get; set; }
    public string? Number { get; set; }
    public int Currency { get; set; }
    public bool Active { get; set; }
}
