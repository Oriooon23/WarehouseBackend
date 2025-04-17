namespace Warehouse.Data.Models
{
    public class RolePermissions
    {
        public int IdRole { get; set; }
        public Roles Role { get; set; }
        public int IdPermission { get; set; }
        public Permissions Permission { get; set; }
    }
}