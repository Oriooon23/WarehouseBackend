namespace Warehouse.Data.Models
{
    public class OrderStatuses : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Orders> Orders { get; set; }
    }
}