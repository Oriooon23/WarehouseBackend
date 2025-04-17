using Warehouse.Common.Responses;


namespace Warehouse.Interfaces.ServicesInterfaces
{
    public interface IGenericService<TDto> where TDto : class
    {
        Task<ResponseBase<IEnumerable<TDto>>> GetAllAsync();
        Task<ResponseBase<TDto>> GetByIdAsync(int id);
    }
}