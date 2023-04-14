using System.Security.Claims;

namespace webapi.SharedServices.Authentication
{
    public interface IAuthenticationService
    {
        string GetToken(IEnumerable<Claim> claims);
    }
}
