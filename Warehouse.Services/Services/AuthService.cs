using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Common.DTOs.UserDTO;
using Warehouse.Common.Responses;
using Warehouse.Data.Models;
using Warehouse.Interfaces.RepositoryInterfaces;
using Warehouse.Interfaces.ServicesInterfaces; // Assicurati che IAuthService sia qui
using Warehouse.Common.Security.SecurityInterface.PasswordInterface; // Namespace di IPasswordService
using Warehouse.Common.Security.SecurityInterface.JwtInterface; // Namespace di IJwtService

namespace Warehouse.Services.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService<Users> _passwordService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService; // Inietta IJwtService

        public AuthService(IUserRepository userRepository, IPasswordService<Users> passwordService, IConfiguration configuration, IMapper mapper, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _configuration = configuration;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        public async Task<ResponseBase<AuthResponseDTO>> LoginAsync(UserLoginDTO loginDto)
        {
            if (loginDto == null || string.IsNullOrWhiteSpace(loginDto.UsernameOrEmail) || string.IsNullOrWhiteSpace(loginDto.Password))
            {
                return ResponseBase<AuthResponseDTO>.Fail("Credenziali non valide.", ErrorCode.InvalidCredentials);
            }

            var user = await _userRepository.GetSingleOrDefaultAsync(u => u.UserName == loginDto.UsernameOrEmail || u.Email == loginDto.UsernameOrEmail);

            if (user == null)
            {
                return ResponseBase<AuthResponseDTO>.Fail("Utente non trovato.", ErrorCode.NotFound);
            }

            if (!_passwordService.VerifyPassword(user, user.PasswordHash, loginDto.Password))
            {
                return ResponseBase<AuthResponseDTO>.Fail("Password errata.", ErrorCode.InvalidCredentials);
            }

            // Genera il token JWT utilizzando il JwtService
            var token = _jwtService.GenerateJwtToken(user);

            // Crea l'AuthResponseDTO con solo il token
            var authResponse = new AuthResponseDTO { Token = token };

            return ResponseBase<AuthResponseDTO>.Success(authResponse);
        }

        public async Task<ResponseBase<UserDTO>> RegisterAsync(UserRegisterDTO registerDto)
        {
            if (registerDto == null)
            {
                return ResponseBase<UserDTO>.Fail("Dati di registrazione non validi.", ErrorCode.BadRequest);
            }

            if (await _userRepository.AnyAsync(u => u.UserName == registerDto.UserName))
            {
                return ResponseBase<UserDTO>.Fail("Nome utente già esistente.", ErrorCode.Duplicate);
            }

            if (await _userRepository.AnyAsync(u => u.Email == registerDto.Email))
            {
                return ResponseBase<UserDTO>.Fail("Email già esistente.", ErrorCode.Duplicate);
            }

            var newUser = _mapper.Map<Users>(registerDto);
            newUser.PasswordHash = _passwordService.HashPassword(newUser, registerDto.Password);
            newUser.CreatedAt = DateTime.UtcNow;

            await _userRepository.AddAsync(newUser);
            await _userRepository.SaveChangesAsync();

            var userDto = _mapper.Map<UserDTO>(newUser);
            return ResponseBase<UserDTO>.Success(userDto);
        }

        public async Task<ResponseBase<bool>> ChangePasswordAsync(UserChangePasswordDTO changePasswordDto)
        {
            if (changePasswordDto == null)
            {
                return ResponseBase<bool>.Fail("Dati per il cambio password non validi.", ErrorCode.BadRequest);
            }

            var user = await _userRepository.GetByIdAsync(changePasswordDto.IdUser);
            if (user == null)
            {
                return ResponseBase<bool>.Fail("Utente non trovato.", ErrorCode.NotFound);
            }

            if (!_passwordService.VerifyPassword(user, user.PasswordHash, changePasswordDto.CurrentPassword))
            {
                return ResponseBase<bool>.Fail("La password attuale non è corretta.", ErrorCode.InvalidCredentials);
            }

            if (changePasswordDto.NewPassword != changePasswordDto.ConfirmNewPassword)
            {
                return ResponseBase<bool>.Fail("Le nuove password non corrispondono.", ErrorCode.BadRequest);
            }

            var newPasswordHash = _passwordService.HashPassword(user, changePasswordDto.NewPassword);
            user.PasswordHash = newPasswordHash;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return ResponseBase<bool>.Success(true);
        }
    }
}