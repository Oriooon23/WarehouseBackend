using Warehouse.Interfaces.RepositoryInterfaces;
using Warehouse.Interfaces.ServicesInterfaces;
using Warehouse.Common.Responses;
using AutoMapper;
using Warehouse.Common.DTOs;
using Warehouse.Data.Models;
using Microsoft.AspNetCore.Http;
using Warehouse.Common.Security;

namespace Warehouse.Services.Services
{
    public class CityService : GenericService<Cities, CityDTO>, ICityService
    {
        private readonly ICityRepository _cityRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CityService(ICityRepository cityRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(cityRepository, mapper)
        {
            _cityRepository = cityRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseBase<CityDTO>> CreateCityAsync(CityDTO cityDto)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user != null && user.IsInRole(Policies.Admin))
            {
                try
                {
                    var cityEntity = _mapper.Map<Cities>(cityDto);
                    var addedCity = await _cityRepository.AddAsync(cityEntity);
                    var addedCityDto = _mapper.Map<CityDTO>(addedCity);
                    return ResponseBase<CityDTO>.Success(addedCityDto);
                }
                catch (Exception ex)
                {
                    return ResponseBase<CityDTO>.Fail($"Errore durante la creazione della città: {ex.Message}", ErrorCode.InternalError);
                }
            }
            else
            {
                return ResponseBase<CityDTO>.Fail("Non autorizzato a creare città.", ErrorCode.Unauthorized);
            }
        }

        public async Task<ResponseBase<CityDTO>> UpdateCityAsync(int id, CityDTO cityDto)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user != null && user.IsInRole(Policies.Admin))
            {
                try
                {
                    var existingCity = await _cityRepository.GetByIdAsync(id);
                    if (existingCity == null)
                    {
                        return ResponseBase<CityDTO>.Fail("Città non trovata.", ErrorCode.NotFound);
                    }

                    _mapper.Map(cityDto, existingCity); // Aggiorna le proprietà dell'entità esistente
                    await _cityRepository.UpdateAsync(existingCity);
                    var updatedCityDto = _mapper.Map<CityDTO>(existingCity);
                    return ResponseBase<CityDTO>.Success(updatedCityDto);
                }
                catch (Exception ex)
                {
                    return ResponseBase<CityDTO>.Fail($"Errore durante l'aggiornamento della città: {ex.Message}", ErrorCode.InternalError);
                }
            }
            else
            {
                return ResponseBase<CityDTO>.Fail("Non autorizzato a modificare le città.", ErrorCode.Unauthorized);
            }
        }
    }
}
