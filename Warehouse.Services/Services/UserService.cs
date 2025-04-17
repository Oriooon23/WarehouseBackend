using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Warehouse.Common.DTOs.UserDTO;
using Warehouse.Common.Responses;
using Warehouse.Data.Models;
using Warehouse.Interfaces.RepositoryInterfaces;
using Warehouse.Interfaces.ServicesInterfaces;

namespace Warehouse.Services.Services
{
    public class UserService : GenericService<Users, UserDTO>, IUserService
    {
        private readonly IUserRepository _userRepository;
        


        public UserService(IUserRepository userRepository, IMapper mapper) : base (userRepository, mapper)
        {
            _userRepository = userRepository;
        }
        public async Task<ResponseBase<UserDTO>> UpdateUserProfileAsync(int userId, UserUpdateDTO model)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return ResponseBase<UserDTO>.Fail("Utente non trovato.", ErrorCode.NotFound);
            }

            _mapper.Map(model, user);

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return ResponseBase<UserDTO>.Success(_mapper.Map<UserDTO>(user));
        }
        public async Task<ResponseBase<bool>> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return ResponseBase<bool>.Fail("Utente non trovato.", ErrorCode.NotFound);
            }

            await _userRepository.DeleteAsync(id);
            await _userRepository.SaveChangesAsync();
            return ResponseBase<bool>.Success(true);
        }
        public async Task<ResponseBase<UserDTO>> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetSingleOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                return ResponseBase<UserDTO>.Fail("Utente non trovato.", ErrorCode.NotFound);
            }
            return ResponseBase<UserDTO>.Success(_mapper.Map<UserDTO>(user));
        }
        public async Task<ResponseBase<UserDTO>> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetSingleOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return ResponseBase<UserDTO>.Fail("Utente non trovato.", ErrorCode.NotFound);
            }
            return ResponseBase<UserDTO>.Success(_mapper.Map<UserDTO>(user));
        }
    }
}
