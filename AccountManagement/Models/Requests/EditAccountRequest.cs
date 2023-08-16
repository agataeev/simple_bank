namespace AccountManagement.Models.Requests;

public class EditAccountRequest: CreateAccountRequest
{
    public long Id { get; set; }
}