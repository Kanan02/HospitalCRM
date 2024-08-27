namespace Application.Models.Response.Base
{
    public interface IListFilterDto<TEntity, TDto>
    {
        TDto SetDto(TEntity entity);
    }
}
