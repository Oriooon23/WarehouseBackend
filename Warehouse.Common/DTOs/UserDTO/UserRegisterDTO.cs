using System;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Common.DTOs.UserDTO
{
    public class UserRegisterDTO
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public DateOnly? BirthDate { get; set; }
    }
}