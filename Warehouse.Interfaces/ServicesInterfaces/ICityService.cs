using Warehouse.Common.DTOs;
using Warehouse.Common.Responses;

namespace Warehouse.Interfaces.ServicesInterfaces
{
    public interface ICityService : IGenericService<CityDTO>
    {
        Task<ResponseBase<CityDTO>> CreateCityAsync(CityDTO cityDto);
        Task<ResponseBase<CityDTO>> UpdateCityAsync(int id, CityDTO cityDto);
    }
}