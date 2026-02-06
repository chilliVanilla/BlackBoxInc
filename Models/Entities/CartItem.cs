namespace BlackBoxInc.Models.Entities
{
    public class CartItem
    {
        public int CartItemId { get; set; }


        public int UserId { get; set; }
        //public Users User { get; set; } = null!;


        public int ProductId { get; set; }
        public Products? Product { get; set; }
        public int Quantity { get; set; }
    }
}