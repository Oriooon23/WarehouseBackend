namespace Warehouse.Common.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int IdUser { get; set; }
        public string User {  get; set; }
        public int IdStatus { get; set; }
        public string Status { get; set; }
        public List<OrderProductDTO> OrderProducts { get; set; }

    }
}