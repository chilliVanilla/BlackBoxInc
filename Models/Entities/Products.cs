using BlackBoxInc.Models.Enums;

namespace BlackBoxInc.Models.Entities
{
    public class Products
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public required decimal Price { get; set; }
        public DateTime AddedAt { get; set; }
        public required AvailabilityStatus InStock { get; set; }
        public string? ProductDescription { get; set; }
        public int StockCount { get; set; }
    }
}
