namespace Warehouse.Data.Models
{
    public class Cities : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Suppliers> Suppliers { get; set; }
    }
}