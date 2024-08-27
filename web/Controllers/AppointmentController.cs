using Application.Constants;
using Application.Interfaces.IServices;
using Application.Models.Request.Ui;
using Application.Models.Response;
using Application.Models.Response.Base;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Base;

namespace web.Controllers
{
    [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.Superadmin},{RoleConstant.User},{RoleConstant.Doctor}")]
    public class AppointmentController : BaseController<Appointment>
    {
        private IAppointmentService _appointmentService { get; }

        public AppointmentController(IAppointmentService appointmentService) : base(appointmentService) { _appointmentService = appointmentService; }
        [HttpGet]
        public async Task<ApiValueResponse<IReadOnlyList<AppoinmentDto>>> GetAll([FromQuery] AppointmentUi model)
        {
            return new ApiValueResponse<IReadOnlyList<AppoinmentDto>>(await _appointmentService.Get(model, HttpContext.User.Claims));
        }
        [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.Superadmin}")]
        [HttpPut]
        public async Task<ApiValueResponse<Appointment>> Update([FromBody] AppoinmentDto entity)
        {
            var claim = HttpContext.User.Claims.First(c => c.Type == ClaimConstant.UserId);
            entity.LastModifiedBy = claim.Value;

            return new ApiValueResponse<Appointment>(await _appointmentService.Update(entity));
        }
        [HttpPost]
        public async Task<ApiValueResponse<Appointment>> Add([FromBody] AppoinmentDto entity)
        {

            var claim = HttpContext.User.Claims.First(c => c.Type == ClaimConstant.UserId);
            entity.CreatedBy = claim.Value;
            return new ApiValueResponse<Appointment>(await _appointmentService.Add(entity));
        }
    }
}
