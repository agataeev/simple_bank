using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AccountManagement.Managers.Abstraction;
using Microsoft.IdentityModel.Tokens;

namespace AccountManagement.Managers.Implementation;

public class JwtTokenManager: IJwtTokenManager
{
    private readonly string _key;
    private readonly string _issuer;
    private readonly string _audience;

    public JwtTokenManager(string key, string issuer, string audience)
    {
        _key = key;
        _issuer = issuer;
        _audience = audience;
    }

    public async Task<string> GenerateToken(string? username, int expireMinutes = 60)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "user") // You can add more claims if needed
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(expireMinutes),
            signingCredentials: creds
        );

        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }
    
    public async Task<string> ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_key);
        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = _issuer,
            ValidateAudience = true,
            ValidAudience = _audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        }, out var validatedToken);

        var jwtToken = (JwtSecurityToken) validatedToken;
        var username = jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value;

        return await Task.FromResult(username);
    }
}