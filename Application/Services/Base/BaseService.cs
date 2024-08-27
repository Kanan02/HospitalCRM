using Application.Interfaces.IRepository.Base;
using Application.Interfaces.IServices.Base;
using Application.Interfaces.IUoW;
using Application.Models.Request.Base;
using Application.Models.Response.Base;
using Application.Spesifications.Base;

namespace Application.Services.Base
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        public IUoW _unitOfWork { get; }

        protected IRepository<TEntity> Repository { get => _unitOfWork.Repository<TEntity>(); }

        public BaseService(IUoW unitOfWork) => _unitOfWork = unitOfWork;

        public virtual Task<IReadOnlyList<TEntity>> GetAllAsync() => Repository.GetAllAsync();
        public async Task<IReadOnlyList<TDto>> GetAllDtoAsync<TDto>() where TDto : IListFilterDto<TEntity, TDto>
            => ConvertListToDto<TDto>((await GetAllAsync()));

        public virtual async Task<TEntity> Save(TEntity entity)
        {
            Repository.Update(entity);
            await SaveUoW();
            return entity;
        }

        public async Task<IReadOnlyList<TDto>> GetListByFilter<TDto>(BaseReq<TEntity> request) where TDto : IListFilterDto<TEntity, TDto>
        {

            var spec = FilterList(request, new BaseSpecification<TEntity>());
            var result = await Repository.GetAsync(spec);

            return ConvertListToDto<TDto>(result);
            // result.Select(x => ((TDto)Activator.CreateInstance(typeof(TDto))).SetDto(x)).ToList();
        }

        public async Task<IReadOnlyList<TEntity>> GetListByFilter(BaseReq<TEntity> request, bool trackDb = true)
        {
            //List<Expression<Func<TEntity, bool>>> filters = new List<Expression<Func<TEntity, bool>>>();
            //List<Expression<Func<TEntity, object>>> includes = new List<Expression<Func<TEntity, object>>>();
            //List<string> includeStrings = new List<string>();

            var spec = FilterList(request, new BaseSpecification<TEntity>());
            return await Repository.GetAsync(spec, trackDb);
        }
        public async Task<int> GetCountByFilter(BaseReq<TEntity> request)
        {
            var spec = FilterList(request, new BaseSpecification<TEntity>());
            return await Repository.CountAsync(spec);
        }
        protected virtual ISpecification<TEntity> FilterList(BaseReq<TEntity> request, ISpecification<TEntity> spec) => spec;

        protected Task SaveUoW() => _unitOfWork.SaveChangesAsync();

        private IReadOnlyList<TDto> ConvertListToDto<TDto>(IReadOnlyList<TEntity> list) where TDto : IListFilterDto<TEntity, TDto>
            => list.Select(x => ((TDto)Activator.CreateInstance(typeof(TDto))).SetDto(x))
                .ToList();


    }
}
