namespace BlackBoxInc.Models.Entities
{
    public class Cart
    {
        public int CartId { get; set; }
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
