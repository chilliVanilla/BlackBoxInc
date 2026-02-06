using System.ComponentModel.DataAnnotations;

namespace BlackBoxInc.Models.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
