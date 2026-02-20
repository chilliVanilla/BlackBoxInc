using Microsoft.Build.Framework;

namespace BlackBoxInc.Models.DTOs
{
    public class TokenRequestDto
    {
        [Required]
        public string AccessToken { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}
