using BlackBoxInc.Models.Entities;

namespace BlackBoxInc.Services
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(User user);
    }
}
