using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Context;
using Warehouse.Data.Models;
using Warehouse.Repositories.Repositories;
using System.Threading.Tasks;
using Warehouse.Interfaces.RepositoryInterfaces;

namespace Warehouse.Repositories.Repositories
{
    public class CategoryRepository : GenericRepository<Categories>, ICategoryRepository
    {

        public CategoryRepository(WarehouseDbContext dbContext) : base(dbContext)
        { }
    }
}
