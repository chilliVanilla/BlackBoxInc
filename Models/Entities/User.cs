using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BlackBoxInc.Models.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}

