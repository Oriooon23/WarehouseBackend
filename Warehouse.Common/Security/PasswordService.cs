using Microsoft.AspNetCore.Identity;
using Warehouse.Common.Security.SecurityInterface.PasswordInterface;

namespace Warehouse.Common.Security
{
    public class PasswordService<TUser> : IPasswordService<TUser> where TUser : class
    {
        private readonly PasswordHasher<TUser> _passwordHasher;
        public PasswordService(PasswordHasher<TUser> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public string HashPassword(TUser user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }

        public bool VerifyPassword(TUser user, string hashedPassword, string providedpassword)
        {
            return _passwordHasher.VerifyHashedPassword(user, hashedPassword, providedpassword) == PasswordVerificationResult.Success;
        }
    }
}