using Application.Constants;
using Application.Interfaces.IServices;
using Application.Models.Request.Base;
using Application.Models.Request.Ui;
using Application.Models.Response;
using Application.Models.Response.Base;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Base;

namespace web.Controllers
{
    [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.Superadmin}")]
    public class SpecialitiesController : BaseController<Speciality>
    {
        private ISpecialityService _specialityService { get; }

        public SpecialitiesController(ISpecialityService specialityService) : base(specialityService) { _specialityService = specialityService; }
        [HttpGet]
        public async Task<ApiValueResponse<PagedResponseList<SpecialityDto>>> GetAll([FromQuery] PagingUi pager) => new ApiValueResponse<PagedResponseList<SpecialityDto>>(await _specialityService.Get(pager));
        [HttpPost]
        public async Task<ApiValueResponse<Speciality>> Add([FromBody] SpecialityDto entity)
        {

            var claim = HttpContext.User.Claims.First(c => c.Type == ClaimConstant.UserId);
            entity.CreatedBy = claim.Value;
            return new ApiValueResponse<Speciality>(await _specialityService.Add(entity));
        }

    }
}
