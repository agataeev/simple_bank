using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AccountManagement.Models.Common;
using AccountManagement.Models.Entities;
using AccountManagement.Models.Responses;
using AccountManagement.Repositories.Abstraction;
using AccountManagement.Services.Abstraction;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using ILogger = Serilog.ILogger;

namespace AccountManagement.Services.Implementation;

public class AuthenticationManager : IAuthenticationManager
{
    private readonly ILogger _logger;
    private readonly IRefreshTokenRepo _refreshTokenRepo;
    private readonly IUserRepo _userRepo;

    public AuthenticationManager(ILogger logger, IRefreshTokenRepo refreshTokenRepo, IUserRepo userRepo)
    {
        _logger = logger;
        _refreshTokenRepo = refreshTokenRepo;
        _userRepo = userRepo;
    }

    private async Task<Tuple<string, List<string>>> AuthenticateAsync(string login, string password)
    {
        _logger.Information("Try login: {login}", login);
        var identity = await GetIdentityAsync(login, password);

        var accessToken = GenerateToken(identity, AuthOptions.AccessTokenLifetime);
        var permissions = accessToken.Claims.Where(u => u.Type == ClaimsIdentity.DefaultRoleClaimType)
            .Select(u => u.Value).ToList();
        return new Tuple<string, List<string>>(EncodeTokenToString(accessToken), permissions);
    }

    public async Task<JwtExtendedResponse?> AuthenticateWithRefreshAsync(string login, string password)
    {
        var jwt = await AuthenticateAsync(login, password);
        if (jwt == null)
        {
            throw new Exception(@"Пользователь с таким Логин \ Пароль не найден");
        }

        if (string.IsNullOrEmpty(jwt.Item1))
        {
            return null;
        }

        Log.Information("Login succeeded for {login}", login);
        var refreshIdentity = GenerateRefreshTokenIdentity(login);
        var refreshToken = EncodeTokenToString(GenerateToken(refreshIdentity, TimeSpan.FromDays(1)));
        await SaveRefreshToken(login, refreshToken);
        return new JwtExtendedResponse(jwt.Item1)
        {
            AccessTokenExpiration = DateTime.Now.Add(AuthOptions.AccessTokenLifetime), RefreshToken = refreshToken,
            Permission = jwt.Item2
        };
    }

    public async Task<JwtExtendedResponse?> RefreshAccessToken(string refreshToken)
    {
        JwtSecurityToken? token;
        try
        {
            token = ReadToken(refreshToken);
        }
        catch (Exception e)
        {
            Log.Warning("ReadToken NULL value: {0}", e.Message);
            return null;
        }

        var dNow = DateTime.Now;
        if (token != null)
        {
            var vFrom = TimeZoneInfo.ConvertTimeFromUtc(token.ValidFrom, TimeZoneInfo.Local);
            var vTo = TimeZoneInfo.ConvertTimeFromUtc(token.ValidTo, TimeZoneInfo.Local);

            if (vFrom >= dNow || vTo <= dNow)
            {
                throw new Exception("Срок действия токена истек. Пройдите авторизацию");
            }
        }

        var tokenEntity = await _refreshTokenRepo.GetRefreshTokenByTokenAsync(refreshToken);
        if (tokenEntity == null)
        {
            throw new Exception("Неизвестный токен");
        }

        if (tokenEntity.Revoked)
        {
            throw new Exception("Токен был отозван");
        }

        var user = await _userRepo.GetUserById(tokenEntity.UserId);
        if (user == null)
        {
            throw new Exception("Пользователь не найден");
        }

        if (user.Username == null) throw new Exception("Пользователь не найден");
        var accessTokenIdentity = GenerateAccessTokenIdentity(user.Username);
        var accessToken = GenerateToken(accessTokenIdentity, AuthOptions.AccessTokenLifetime);
        var newRefreshTokenIdentity = GenerateRefreshTokenIdentity(user.Username);
        var newRefreshToken = GenerateToken(newRefreshTokenIdentity, TimeSpan.FromDays(1));

        tokenEntity.Value = EncodeTokenToString(newRefreshToken);
        tokenEntity.Created = DateTime.Now.ToUniversalTime();
        await _refreshTokenRepo.SaveRefreshToken(tokenEntity);

        return new JwtExtendedResponse(EncodeTokenToString(accessToken))
        {
            RefreshToken = EncodeTokenToString(newRefreshToken),
            AccessTokenExpiration = DateTime.Now.Add(AuthOptions.AccessTokenLifetime)
        };

    }

    private JwtSecurityToken? ReadToken(string jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        try
        {
            handler.ValidateToken(jwt, new TokenValidationParameters
            {
                ValidateIssuer = false,
                // ValidIssuer = AuthOptions.ISSUER,
                ValidateAudience = false,
                // ValidAudience = AuthOptions.AUDIENCE,
                ValidateLifetime = true,
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                ValidateIssuerSigningKey = true,
            }, out _);
        }
        catch (SecurityTokenException e)
        {
            throw new Exception("Принятый токен невалиден: " + e.Message);
        }

        return handler.ReadJwtToken(jwt);
    }

    private async Task<ClaimsIdentity> GetIdentityAsync(string userName, string password)
    {
        return GenerateAccessTokenIdentity(userName);
    }

    private ClaimsIdentity GenerateAccessTokenIdentity(string userName)
    {
        var claims = new List<Claim> {new(ClaimsIdentity.DefaultNameClaimType, userName)};

        var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
        return claimsIdentity;
    }

    private ClaimsIdentity GenerateRefreshTokenIdentity(string userName)
    {
        var claims = new List<Claim> {new Claim(ClaimsIdentity.DefaultNameClaimType, userName)};
        var claimsIdentity = new ClaimsIdentity(claims, "RefreshToken", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
        return claimsIdentity;
    }

    private JwtSecurityToken GenerateToken(ClaimsIdentity identity, TimeSpan duration)
    {
        var now = DateTime.Now;
        var jwt = new JwtSecurityToken(
            // issuer: AuthOptions.ISSUER,
            // audience: AuthOptions.AUDIENCE,
            notBefore: now,
            claims: identity.Claims,
            expires: now.Add(duration),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));
        return jwt;
    }

    private string EncodeTokenToString(JwtSecurityToken jwt)
    {
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        return encodedJwt;
    }

    private async Task SaveRefreshToken(string login, string token)
    {
        var user = await _userRepo.GetUserByUsername(login);
        if (user != null)
        {
            var refreshToken = new RefreshToken
            {
                Created = DateTime.Now.ToUniversalTime(),
                Value = token,
                UserId = user.Id
            };
            await _refreshTokenRepo.SaveRefreshToken(refreshToken);
        }
    }
}