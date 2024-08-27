using Application.Models.Request.Base;
using Application.Models.Request.Ui;
using Domain.Entities.Security;
using System.Linq.Expressions;

namespace Application.Models.Request.Security
{
    public class UserReq : PagingReq<User>
    {
        public bool IncludeRole { get; set; }
        public bool IncludeClinics { get; set; }
        public bool IncludeSpecialities { get; set; }
        public string? Password { get; set; }
        public Expression<Func<User, object>>? orderByDescExpression;
        public UserReq() { }

        public UserReq(SignInUi req)
        {
            Value = new User
            {
                Msisdn = req.Msisdn,
            };
            Password = req.Password;
        }
        public UserReq(UserUi req)
        {
            Value = new User
            {
                Msisdn = req.Msisdn,
                FirstName = req.FirstName,
                LastName = req.LastName,
                Patronymic = req.Patronymic,
                Status = req.Status,

            };
            Pager = req.CurrentPage == 0 && req.PageSize == 0 ? null : new PagingOptions
            {
                PageSize = req.PageSize,
                CurrentPage = req.CurrentPage
            };
            Password = req.Password;
        }
    }
}
