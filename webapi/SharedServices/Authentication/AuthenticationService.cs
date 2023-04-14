using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace webapi.SharedServices.Authentication;

public class AuthenticationService : IAuthenticationService
{
    internal struct AppAuthConstants
    {
        public const string Issuer = "https://localhost:7194";
        public const string Audience = Issuer;
        public const string Secret = "my_special_secret";
    }
    public string GetToken(IEnumerable<Claim> claims)
    {
        byte[] secretBytes = Encoding.UTF8.GetBytes(AppAuthConstants.Secret);

        var securityKey = new SymmetricSecurityKey(secretBytes);

        var token = new JwtSecurityToken(
            issuer: AppAuthConstants.Issuer, 
            audience: AppAuthConstants.Audience,
            expires: DateTime.Now.AddMinutes(5),
            notBefore: DateTime.Now,
            claims: claims,
            signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256));

        var tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenAsString;
    }
}