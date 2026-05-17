namespace WarehouseData.Models
{
    public class Product
    {
        public int Id { get; set; }

        public int WarehouseId { get; set; }

        public string PhotoPath { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Manufacturer { get; set; } = string.Empty;

        public string Supplier { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public decimal DiscountPercent { get; set; }

        public Product()
        {
        }
    }
}



