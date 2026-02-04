using BlackBoxInc.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace BlackBoxInc.Models.Entities
{
    public class Products
    {
        [Key]
        public int ProductId { get; set; }
        public required string Name { get; set; }
        public required decimal Price { get; set; }
        public DateTime AddedAt { get; set; }
        public required AvailabilityStatus InStock { get; set; }
        public string? ProductDescription { get; set; }
        public int StockCount { get; set; }
    }
}
