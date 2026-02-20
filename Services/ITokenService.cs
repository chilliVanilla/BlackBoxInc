using System.Security.Claims;
using BlackBoxInc.Models.Entities;

namespace BlackBoxInc.Services
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(User user);
        public string GenerateRefreshToken();
        public ClaimsPrincipal GetPrincipalFromJwtAccessToken(string token);
    }
}
