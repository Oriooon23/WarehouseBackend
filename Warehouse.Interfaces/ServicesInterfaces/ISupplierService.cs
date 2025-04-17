using Warehouse.Common.DTOs;
using Warehouse.Common.Responses;

namespace Warehouse.Interfaces.ServicesInterfaces
{
    public interface ISupplierService : IGenericService<SupplierDTO>
    {
        Task<ResponseBase<SupplierDTO>> CreateSupplierAsync(SupplierDTO supplierDto);
        Task<ResponseBase<bool>> DeleteSupplierAsync(int id);
        Task<ResponseBase<SupplierDTO>> UpdateSupplierAsync(int id, SupplierDTO supplierDto);
    }
}