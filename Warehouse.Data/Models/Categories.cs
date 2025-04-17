namespace Warehouse.Data.Models
{
    public class Categories : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Products> Products { get; set; }
    }
}