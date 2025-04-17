namespace Warehouse.Common.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public int StockQuantity { get; set; }
        public int IdCategory { get; set; }
        public string Category { get; set; }
        public int IdSupplier { get; set; }
        public string Supplier { get; set; }
    }
}