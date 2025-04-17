using Warehouse.Common.DTOs;
using Warehouse.Common.Responses;
using Warehouse.Data.Models;
using Warehouse.Interfaces.RepositoryInterfaces;
using Warehouse.Interfaces.ServicesInterfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Warehouse.Common.Security;

namespace Warehouse.Services.Services
{
    public class CategoryService : GenericService<Categories, CategoryDTO>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(categoryRepository, mapper)
        {
            _categoryRepository = categoryRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseBase<CategoryDTO>> CreateCategoryAsync(CategoryDTO categoryDto)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user != null && user.IsInRole(Policies.Admin))
            {
                try
                {
                    var categoryEntity = _mapper.Map<Categories>(categoryDto);
                    var addedCategory = await _categoryRepository.AddAsync(categoryEntity);
                    var addedCategoryDto = _mapper.Map<CategoryDTO>(addedCategory);
                    return ResponseBase<CategoryDTO>.Success(addedCategoryDto);
                }
                catch (Exception ex)
                {
                    return ResponseBase<CategoryDTO>.Fail($"Errore durante la creazione della categoria: {ex.Message}", ErrorCode.InternalError);
                }
            }
            else
            {
                return ResponseBase<CategoryDTO>.Fail("Non autorizzato a creare categorie.", ErrorCode.Unauthorized);
            }
        }

        public async Task<ResponseBase<CategoryDTO>> DeleteCategoryAsync(int id, CategoryDTO categoryDTO)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user != null && user.IsInRole(Policies.Admin))
            {
                try
                {
                    var categoryToDelete = await _categoryRepository.GetByIdAsync(id);
                    if (categoryToDelete == null)
                    {
                        return ResponseBase<CategoryDTO>.Fail("Categoria non trovata.", ErrorCode.NotFound);
                    }
                    await _categoryRepository.DeleteAsync(id);
                    return ResponseBase<CategoryDTO>.Success(null);
                }
                catch (Exception ex)
                {
                    return ResponseBase<CategoryDTO>.Fail($"Errore durante l'eliminazione della categoria: {ex.Message}", ErrorCode.InternalError);
                }
            }
            else
            {
                return ResponseBase<CategoryDTO>.Fail("Non autorizzato a eliminare categorie.", ErrorCode.Unauthorized);
            }
        }

        public async Task<ResponseBase<CategoryDTO>> UpdateCategoryAsync(int id, CategoryDTO categoryDto)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user != null && user.IsInRole(Policies.Admin))
            {
                try
                {
                    var existingCategory = await _categoryRepository.GetByIdAsync(id);
                    if (existingCategory == null)
                    {
                        return ResponseBase<CategoryDTO>.Fail("Categoria non trovata.", ErrorCode.NotFound);
                    }

                    _mapper.Map(categoryDto, existingCategory);

                    await _categoryRepository.UpdateAsync(existingCategory);
                    var updatedCategoryDto = _mapper.Map<CategoryDTO>(existingCategory);
                    return ResponseBase<CategoryDTO>.Success(updatedCategoryDto);
                }
                catch (Exception ex)
                {
                    return ResponseBase<CategoryDTO>.Fail($"Errore durante l'aggiornamento della categoria: {ex.Message}", ErrorCode.InternalError);
                }
            }
            else
            {
                return ResponseBase<CategoryDTO>.Fail("Non autorizzato a modificare le categorie.", ErrorCode.Unauthorized);
            }
        }
    }
}