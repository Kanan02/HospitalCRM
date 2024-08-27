using Application.Interfaces.IServices.Base;
using Application.Models.Request.Base;
using Application.Models.Request.Ui;
using Application.Models.Response;
using Application.Models.Response.Base;
using Domain.Entities;

namespace Application.Interfaces.IServices
{
    public interface ISpecialityService : IBaseService<Speciality>
    {
        Task<PagedResponseList<SpecialityDto>> Get(PagingUi pager);
        Task<Speciality> Add(SpecialityDto model);
    }
}
