namespace AccountManagement.Models.Responses;

public class JwtExtendedResponse: JwtResponse
{
    public string RefreshToken { get; set; }
    public DateTime AccessTokenExpiration { get; set; }
    public IEnumerable<string> Permission { get; set; }

    public JwtExtendedResponse(string jwt) : base(jwt)
    {
    }
}