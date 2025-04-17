using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Models;
using Warehouse.Interfaces.RepositoryInterfaces;
using System.Linq.Expressions;
using Warehouse.Data.Context;

namespace Warehouse.Repositories.Repositories // Assumi questo sia il namespace dei tuoi repository concreti
{
    public class UserRepository : GenericRepository<Users>, IUserRepository
    {

        public UserRepository(WarehouseDbContext context) : base(context)
        {
        }

        public async Task<Users> GetSingleOrDefaultAsync(Expression<Func<Users, bool>> predicate)
        {
            return await _context.Set<Users>().Where(predicate).SingleOrDefaultAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<Users, bool>> predicate)
        {
            return await _context.Set<Users>().AnyAsync(predicate);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}