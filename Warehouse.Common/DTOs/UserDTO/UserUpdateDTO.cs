namespace Warehouse.Common.DTOs.UserDTO
{
    public class UserUpdateDTO
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateOnly? BirthDate { get; set; }
        public int? IdSupplier { get; set; }
    }
}