namespace AccountManagement.Models.Entities;

public class Transaction: Base
{
    public long? AccountId { get; set; }
    public long? UserId { get; set; }
    public DateTime? Created { get; set; }
    public int Type { get; set; }
    public decimal? Amount { get; set; }
    public decimal? Balance { get; set; }
    public long? ToAccountId { get; set; }
}