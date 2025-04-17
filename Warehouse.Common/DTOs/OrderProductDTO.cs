namespace Warehouse.Common.DTOs
{
    public class OrderProductDTO
    {
        public int IdOrder { get; set; }
        public int IdProduct { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}