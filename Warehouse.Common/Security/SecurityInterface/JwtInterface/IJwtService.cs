using Warehouse.Data.Models;

namespace Warehouse.Common.Security.SecurityInterface.JwtInterface
{
    public interface IJwtService
    {
        string GenerateJwtToken(Users user);
    }
}