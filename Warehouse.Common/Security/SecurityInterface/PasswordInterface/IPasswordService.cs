namespace Warehouse.Common.Security.SecurityInterface.PasswordInterface
{
    public interface IPasswordService<TUser> where TUser : class
    {
        string HashPassword(TUser user, string password);
        bool VerifyPassword(TUser user, string hashedPassword, string providedpassword);
    }
}
