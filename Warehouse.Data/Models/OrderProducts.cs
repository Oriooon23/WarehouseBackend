namespace Warehouse.Data.Models
{
    public class OrderProducts
    {
        public int IdOrder { get; set; }
        public Orders Order { get; set; }
        public int IdProduct { get; set; }
        public Products Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}