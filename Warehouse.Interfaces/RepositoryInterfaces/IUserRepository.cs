using Warehouse.Data.Models;
using Warehouse.Interfaces.RepositoryInterfaces;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Warehouse.Interfaces.RepositoryInterfaces
{
    public interface IUserRepository : IGenericRepository<Users>
    {
        Task<Users> GetSingleOrDefaultAsync(Expression<Func<Users, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<Users, bool>> predicate);
        Task SaveChangesAsync();
    }
}