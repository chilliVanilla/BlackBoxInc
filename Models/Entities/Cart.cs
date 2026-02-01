namespace BlackBoxInc.Models.Entities
{
    public class Cart
    {
        public int ID { get; set; }
        public Products? Products;
        public decimal Total;
    }
}
