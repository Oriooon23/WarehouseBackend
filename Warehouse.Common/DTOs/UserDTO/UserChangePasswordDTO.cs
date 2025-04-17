using System.ComponentModel.DataAnnotations;

namespace Warehouse.Common.DTOs.UserDTO
{
    public class UserChangePasswordDTO
    {
        public int IdUser { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}