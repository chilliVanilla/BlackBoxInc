using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BlackBoxInc.Models.Entities
{
    public class User : IdentityUser
    {
        [Key]
        public int UserId { get; set; }
        // Add the user details input when I have properly understood authentication and authorization
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}

