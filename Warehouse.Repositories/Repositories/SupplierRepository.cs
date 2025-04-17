using Warehouse.Data.Models;
using Warehouse.Interfaces.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Context;

namespace Warehouse.Repositories.Repositories
{
    public class SupplierRepository : GenericRepository<Suppliers>, ISupplierRepository
    {
        public SupplierRepository(WarehouseDbContext context) : base(context)
        { }
    }
}