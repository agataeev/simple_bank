using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AccountManagement.Models.Common;

public class AuthOptions
{
    public const string ISSUER = "simplebank";
    public const string AUDIENCE = "simplebank.kz";
    const string KEY = "simplebank the most secret key in the world";
    public static readonly TimeSpan AccessTokenLifetime = TimeSpan.FromHours(1);

    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}