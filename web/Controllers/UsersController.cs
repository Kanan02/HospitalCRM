using Application.Constants;
using Application.Interfaces.IServices.Security;
using Application.Models.Request.Ui;
using Application.Models.Response;
using Application.Models.Response.Base;
using Domain.Entities.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Base;

namespace web.Controllers
{
    [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.Superadmin}")]
    public class UsersController : BaseController<User>
    {
        private IUserService _userService { get; }


        public UsersController(IUserService userService) : base(userService)
        {
            _userService = userService;
        }
        //public async Task<ApiValueResponse<User>> Add([FromBody] UserUi request)
        //{
        //    return new ApiValueResponse<LoginRes>(await _userService.Add(request));
        //}
        [HttpPost]
        public async Task<ApiValueResponse<User>> Add([FromBody] UserUi entity)
        {

            var claim = HttpContext.User.Claims.First(c => c.Type == ClaimConstant.UserId);
            entity.CreatedBy = claim.Value;

            return new ApiValueResponse<User>(await _userService.Add(entity));
        }
        [HttpPut]
        public async Task<ApiValueResponse<User>> Update([FromBody] UserUi entity)
        {
            var claim = HttpContext.User.Claims.First(c => c.Type == ClaimConstant.UserId);
            entity.LastModifiedBy = claim.Value;

            return new ApiValueResponse<User>(await _userService.Update(entity));
        }
        [HttpGet]
        public async Task<ApiValueResponse<PagedResponseList<UserDto>>> GetAll([FromQuery] UserUi pager) => new ApiValueResponse<PagedResponseList<UserDto>>(await _userService.Get(pager));



    }
}
