using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Context;
using Warehouse.Data.Models;
using Warehouse.Interfaces.RepositoryInterfaces;

namespace Warehouse.Repositories.Repositories
{
    public class CityRepository : GenericRepository<Cities>, ICityRepository
    {
        public CityRepository(WarehouseDbContext context) : base(context)
        { }
        
    }
}