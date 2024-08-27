using Application.Interfaces.IServices.Base;
using Application.Models.Request.Security;
using Application.Models.Request.Ui;
using Application.Models.Response;
using Application.Models.Response.Base;
using Domain.Entities.Security;

namespace Application.Interfaces.IServices.Security
{
    public interface IUserService : IBaseService<User>
    {
        Task<LoginRes> SignIn(UserReq req);
        Task<LoginRes> RefreshToken(ManageTokenReq token);
        Task<bool> SignOut(ManageTokenReq model);
        Task<LoginRes> SignUp(SignUpReq req, string role);

        Task<bool> ResetPassword(ResetPwdReq req);
        Task<User> Add(UserUi req);
        Task<User> Update(UserUi req);
        Task<PagedResponseList<UserDto>> Get(UserUi req);
    }
}
