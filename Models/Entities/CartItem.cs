using System.ComponentModel.DataAnnotations.Schema;

namespace BlackBoxInc.Models.Entities
{
    public class CartItem
    {
        public int CartItemId { get; set; }


        public required string UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;


        public int ProductId { get; set; }
        public Products? Product { get; set; }
        public int Quantity { get; set; }
    }
}