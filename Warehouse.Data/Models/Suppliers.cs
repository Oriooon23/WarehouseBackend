namespace Warehouse.Data.Models 
{
    public class Suppliers : BaseEntity
    {
        public string Name { get; set; }
        public int? IdCity { get; set; }
        public Cities City { get; set; }
        public ICollection<Users> Users { get; set; }
        public ICollection<Products> Products { get; set; }
    }
}