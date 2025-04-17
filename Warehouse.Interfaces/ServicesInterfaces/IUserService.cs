using Warehouse.Common.DTOs.UserDTO;
using Warehouse.Common.Responses;

namespace Warehouse.Interfaces.ServicesInterfaces
{
    public interface IUserService : IGenericService<UserDTO>
    {
        Task<ResponseBase<bool>> DeleteUserAsync(int id);
        Task<ResponseBase<UserDTO>> GetUserByEmailAsync(string email);
        Task<ResponseBase<UserDTO>> GetUserByUsernameAsync(string username);
        Task<ResponseBase<UserDTO>> UpdateUserProfileAsync(int userId, UserUpdateDTO model);
    }
}
