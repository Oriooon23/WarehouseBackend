namespace Warehouse.Data.Models
{
    public class Roles : BaseEntity
    {
        public string Role { get; set; }
        public ICollection<Users> Users { get; set; }
        public ICollection<RolePermissions> RolePermissions { get; set; } // Relazione molti-a-molti con Permission
    }
}