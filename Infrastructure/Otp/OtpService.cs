using Application.Interfaces.IServices.ApiBase;
using Application.Interfaces.IServices.Sms;
using Application.Models.Request.Sms;
using Application.Models.Response.Sms;
using Domain.Exceptions;

namespace Infrastructure.Otp;

public class OtpService : IOtpService
{
    private readonly IApiBaseService _apiBaseService;
    public const string ClientName = "OtpAPI";

    public OtpService(IApiBaseService apiBaseService)
    {
        _apiBaseService = apiBaseService;
    }

    public async Task<OtpBaseRes<bool>> SendCode(SendOtpReq req)
    {
        req.ServiceName = "sozoyunu";
        var result = await _apiBaseService.PostResourceAsync<SendOtpReq, OtpBaseRes<bool>>("send-code", req, ClientName);
        if (!result.Success)
        {
            throw new BadRequestException(result.Errors[0].ErrorMessage);
        }

        return result;
    }

    public async Task<OtpBaseRes<bool>> VerifyCode(VerifyOtpReq req)
    {
        var result =
            await _apiBaseService.PostResourceAsync<VerifyOtpReq, OtpBaseRes<bool>>("verify-code", req, ClientName);

        return result;
    }
}