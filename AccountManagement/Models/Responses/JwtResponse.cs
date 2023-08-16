namespace AccountManagement.Models.Responses;

public class JwtResponse
{
    public string JWT { get; }

    public JwtResponse(string jwt) {
        JWT = jwt;
    }
}