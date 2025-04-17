namespace Warehouse.Data.Models
{
    public class Orders : BaseEntity
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int IdUser { get; set; }
        public Users User { get; set; }
        public int IdStatus { get; set; }
        public OrderStatuses Status { get; set; }
        public ICollection<OrderProducts> OrderProducts { get; set; }
    }
}

