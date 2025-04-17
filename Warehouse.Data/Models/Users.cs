namespace Warehouse.Data.Models
{
    public class Users : BaseEntity
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateOnly? BirthDate { get; set; }
        public int IdRole { get; set; }
        public Roles Role { get; set; }
        public int? IdSupplier { get; set; }
        public Suppliers Supplier { get; set; } 
        public ICollection<Orders> Orders { get; set; }
    }
}