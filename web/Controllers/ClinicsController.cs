using Application.Constants;
using Application.Interfaces.IServices;
using Application.Models.Request.Base;
using Application.Models.Request.Ui;
using Application.Models.Response;
using Application.Models.Response.Base;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Web.Controllers.Base;

namespace web.Controllers
{
    [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.Superadmin}")]
    public class ClinicsController : BaseController<Clinic>
    {
        private IClinicService _clinicService { get; }

        public ClinicsController(IClinicService clinicService) : base(clinicService) { _clinicService = clinicService; }
        [HttpGet]
        public async Task<ApiValueResponse<PagedResponseList<ClinicDto>>> GetAll([FromQuery] PagingUi pager)
        {
            var userId = HttpContext.User.Claims.First(c => c.Type == ClaimConstant.UserId).Value;
            var role = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Role).Value;
            if (role == RoleConstant.Superadmin)
            {
                return new ApiValueResponse<PagedResponseList<ClinicDto>>(await _clinicService.Get(pager));
            }
            return new ApiValueResponse<PagedResponseList<ClinicDto>>(await _clinicService.Get(pager, new Guid(userId)));
        }
        [HttpPost]
        public async Task<ApiValueResponse<Clinic>> Add([FromBody] ClinicDto entity)
        {

            var claim = HttpContext.User.Claims.First(c => c.Type == ClaimConstant.UserId);
            entity.CreatedBy = claim.Value;
            return new ApiValueResponse<Clinic>(await _clinicService.Add(entity));
        }

    }
}
