namespace Warehouse.Data.Models
{
    public class Products : BaseEntity
    {
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public int StockQuantity { get; set; }
        public int IdCategory { get; set; }
        public Categories Category { get; set; }
        public int IdSupplier { get; set; }
        public Suppliers Supplier { get; set; }
        public ICollection<OrderProducts> OrderProducts { get; set; } 
    }
}