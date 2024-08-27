using Application.Models.Request.Base;
using Application.Models.Response.Base;


namespace Application.Interfaces.IServices.Base
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        Task<TEntity> Save(TEntity entity);
        Task<IReadOnlyList<TEntity>> GetAllAsync();
        Task<IReadOnlyList<TDto>> GetAllDtoAsync<TDto>() where TDto : IListFilterDto<TEntity, TDto>;
        Task<IReadOnlyList<TDto>> GetListByFilter<TDto>(BaseReq<TEntity> request) where TDto : IListFilterDto<TEntity, TDto>;
        Task<IReadOnlyList<TEntity>> GetListByFilter(BaseReq<TEntity> request, bool trackDb = true);


    }
}
