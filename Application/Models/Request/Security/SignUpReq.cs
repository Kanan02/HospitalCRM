using Application.Models.Request.Ui;
using Domain.Entities.Security;

namespace Application.Models.Request.Security
{
    public class SignUpReq : UserReq
    {
        public bool IncludeRole { get; set; }
        //public bool IncludeManager { get; set; }

        public string Password { get; set; }
        public SignUpReq() { }

        public SignUpReq(SignUpUi req)
        {
            Value = new User
            {
                Msisdn = req.Msisdn,
            };
            Password = req.Password;
        }
    }
}
