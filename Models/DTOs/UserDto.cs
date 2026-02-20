

namespace BlackBoxInc.Models.DTOs
{
    public class UserDto
    {
        public required string Username { get; set; }
        public string? Email { get; set; }
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
        
    }
}