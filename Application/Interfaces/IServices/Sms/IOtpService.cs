using Application.Models.Request.Sms;
using Application.Models.Response.Sms;

namespace Application.Interfaces.IServices.Sms;

public interface IOtpService
{
    Task<OtpBaseRes<bool>> SendCode(SendOtpReq req);
    Task<OtpBaseRes<bool>> VerifyCode(VerifyOtpReq req);
}