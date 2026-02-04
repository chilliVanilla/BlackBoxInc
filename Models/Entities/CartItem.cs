namespace BlackBoxInc.Models.Entities
{
    public class CartItem
    {
        public int CartItemId { get; set; }



        public int CartId { get; set; }
        public Cart Cart { get; set; } = null!;


        public int ProductId { get; set; }
        public Products? Product { get; set; }


        public required string Name { get; set; }
        public required decimal Price { get; set; }
        public string? ProductDescription { get; set; }

    }
}