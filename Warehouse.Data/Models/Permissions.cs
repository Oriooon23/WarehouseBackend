namespace Warehouse.Data.Models
{
    public class Permissions : BaseEntity
    {
        public string Permission { get; set; }
        public ICollection<RolePermissions> RolePermissions { get; set; } // Relazione molti-a-molti con Role
    }
}