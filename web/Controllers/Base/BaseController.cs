using Application.Interfaces.IServices.Base;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<TEntity> : ControllerBase where TEntity : class
    {
        public IBaseService<TEntity> _service { get; }

        public BaseController(IBaseService<TEntity> service)
        {
            _service = service;
        }
        //[HttpGet]
        //public virtual async Task<ApiValueResponse<IReadOnlyList<TEntity>>> GetAll() => new ApiValueResponse<IReadOnlyList<TEntity>>(await _service.GetAllAsync());
        //[HttpPost]
        //public virtual async Task<ApiValueResponse<TEntity>> Save([FromBody] TEntity entity) => new ApiValueResponse<TEntity>(await _service.Save(entity));
    }
}
