using System.ComponentModel.DataAnnotations;

namespace BlackBoxInc.Models.DTOs
{
    public class SignUpDto
    {
        [Required] public string FirstName{ get; set; }
        //public required string MiddleName { get; set; }
        [Required]public string LastName { get; set; }
        [EmailAddress] [Required] public string Email { get; set; }
        [Required] public string Username { get; set; }
        [MinLength(8)] [Required] public required string password { get; set; }
    }
}
