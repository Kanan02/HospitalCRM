using Application.Interfaces.IServices.Base;
using Application.Models.Request.Ui;
using Application.Models.Response;
using Domain.Entities;
using System.Security.Claims;

namespace Application.Interfaces.IServices
{
    public interface IAppointmentService : IBaseService<Appointment>
    {
        Task<IReadOnlyList<AppoinmentDto>> Get(AppointmentUi pager, IEnumerable<Claim> claims);
        Task<Appointment> Add(AppoinmentDto req);
        Task<Appointment> Update(AppoinmentDto req);
    }
}
