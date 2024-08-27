using Application.Interfaces.IServices.Security;
using Application.Interfaces.IUoW;
using Application.Models.Request.Base;
using Application.Services.Base;
using Application.Spesifications.Base;
using Domain.Entities.Security;

namespace Application.Services.Security
{
    public class RoleService : BaseService<Role>, IRoleService
    {
        public RoleService(IUoW unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<Role> GetByName(string name)
        {
            var req = new BaseReq<Role>
            {
                Value = new Role
                {
                    Name = name
                }
            };
            return (await GetListByFilter(req)).FirstOrDefault();
        }

        protected override ISpecification<Role> FilterList(BaseReq<Role> request, ISpecification<Role> spec)
        {
            var entity = request.Value;

            if (entity != null)
            {

                if (!string.IsNullOrEmpty(entity.Name))
                    spec.Filters.Add(x => x.Name == entity.Name.Trim());

            }
            return base.FilterList(request, spec);
        }
    }
}
