namespace Warehouse.Common.DTOs.UserDTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; } // Aggiornato a CreatedAt
        public DateOnly? BirthDate { get; set; }
        public int IdRole { get; set; }
        public string RoleName { get; set; }
        public int? IdSupplier { get; set; }
        public string SupplierName { get; set; }
    }
}