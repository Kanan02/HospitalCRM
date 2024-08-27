using Application.Interfaces.IServices.Base;
using Domain.Entities.Security;

namespace Application.Interfaces.IServices.Security
{
    public interface IRoleService : IBaseService<Role>
    {
        Task<Role> GetByName(string name);
    }
}
