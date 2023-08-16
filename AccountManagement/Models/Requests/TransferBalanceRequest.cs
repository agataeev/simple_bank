namespace AccountManagement.Models.Requests;

public class TransferBalanceRequest
{
    public long FromId { get; set; }
    public long ToId { get; set; }
    public decimal Amount { get; set; }
}