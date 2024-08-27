using Application.Interfaces.IServices.Base;
using Application.Models.Request.Base;
using Application.Models.Request.Ui;
using Application.Models.Response;
using Application.Models.Response.Base;
using Domain.Entities;

namespace Application.Interfaces.IServices
{
    public interface IClinicService : IBaseService<Clinic>
    {
        Task<PagedResponseList<ClinicDto>> Get(PagingUi pager, Guid? userId = null);
        Task<Clinic> Add(ClinicDto model);
    }
}
