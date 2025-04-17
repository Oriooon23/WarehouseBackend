using Warehouse.Interfaces.RepositoryInterfaces;
using Warehouse.Interfaces.ServicesInterfaces;
using Warehouse.Common.Responses;
using AutoMapper;

namespace Warehouse.Services.Services
{
    public abstract class GenericService<TEntity, TDto> : IGenericService<TDto>
        where TEntity : class
        where TDto : class
    {
        protected readonly IGenericRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public GenericService(IGenericRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<ResponseBase<IEnumerable<TDto>>> GetAllAsync()
        {
            ResponseBase<IEnumerable<TDto>> response;

            try
            {
                var entities = await _repository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<TDto>>(entities);
                response = ResponseBase<IEnumerable<TDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                response = ResponseBase<IEnumerable<TDto>>.Fail($"Errore durante il recupero dei dati: {ex.Message}", ErrorCode.InternalError);
            }

            return response;
        }

        public virtual async Task<ResponseBase<TDto>> GetByIdAsync(int id)
        {
            ResponseBase<TDto> response;

            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                {
                    response = ResponseBase<TDto>.Fail("Elemento non trovato.", ErrorCode.NotFound);
                }
                else
                {
                    var dto = _mapper.Map<TDto>(entity);
                    response = ResponseBase<TDto>.Success(dto);
                }
            }
            catch (Exception ex)
            {
                response = ResponseBase<TDto>.Fail($"Errore durante il recupero dell'elemento: {ex.Message}", ErrorCode.InternalError);
            }

            return response;
        }

    }
}
