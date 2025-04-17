using Warehouse.Common.DTOs;
using Warehouse.Common.Responses;

namespace Warehouse.Interfaces.ServicesInterfaces
{
    public interface ICategoryService : IGenericService<CategoryDTO>
    {
        Task<ResponseBase<CategoryDTO>> CreateCategoryAsync(CategoryDTO categoryDto);
        Task<ResponseBase<CategoryDTO>> UpdateCategoryAsync(int id, CategoryDTO categoryDto);
        Task<ResponseBase<CategoryDTO>> DeleteCategoryAsync(int id, CategoryDTO categoryDTO);
    }
}