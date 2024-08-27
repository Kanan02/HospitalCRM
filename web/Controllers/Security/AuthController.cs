using Application.Interfaces.IServices.Security;
using Application.Interfaces.IServices.Sms;
using Application.Models.Request.Security;
using Application.Models.Request.Sms;
using Application.Models.Request.Ui;
using Application.Models.Response;
using Application.Models.Response.Base;
using Domain.Entities.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Base;

namespace web.Controllers.Security
{
    public class AuthController : BaseController<User>
    {
        private IUserService _userService { get; }
        private IOtpService _otpService { get; }

        public AuthController(IUserService userService, IOtpService otpService) : base(userService)
        {
            _userService = userService;
            _otpService = otpService;

        }
        [HttpPost("sign-in")]
        public async Task<ApiValueResponse<LoginRes>> SignIn([FromBody] SignInUi request)
        {
            var req = new UserReq(request);
            return new ApiValueResponse<LoginRes>(await _userService.SignIn(req));
        }
        [HttpPost("refresh-token")]
        public async Task<ApiValueResponse<LoginRes>> RefreshToken([FromBody] ManageTokenReq request)
        {
            return new ApiValueResponse<LoginRes>(await _userService.RefreshToken(request));
        }
        [Authorize]
        [HttpPost("sign-out")]
        public async Task<ApiValueResponse<bool>> SignOut([FromBody] ManageTokenReq request)
        {
            return new ApiValueResponse<bool>(await _userService.SignOut(request));
        }

        [HttpPost("send-code")]
        public async Task<ApiValueResponse<bool>> SendCode([FromBody] SendOtpReq req)
        {
            var res = await _otpService.SendCode(req);
            return new ApiValueResponse<bool>(res.Value);
        }


        [HttpPost("reset-password")]
        public async Task<ApiValueResponse<bool>> ResetPassword([FromBody] ResetPwdReq req)
        {
            return new ApiValueResponse<bool>(await _userService.ResetPassword(req));
        }


        //[AllowAnonymous]
        //[HttpPost("sign-up")]
        //public async Task<ApiValueResponse<LoginRes>> SignUp([FromBody] SignUpUi request)
        //{
        //    var req = new SignUpReq(request);
        //    return new ApiValueResponse<LoginRes>(await _userService.SignUp(req, RoleConstant.Admin));
        //}
    }
}
