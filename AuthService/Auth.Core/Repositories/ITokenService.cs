using Auth.Core.Entities;
using System.Security.Claims;

namespace Auth.Core.Repositories
{
    public interface ITokenService
    {
        Task<string> GenerateAccessTokenAsync(ApplicationUser user);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
        Task<bool> ValidateTokenAsync(string token);
    }
}
