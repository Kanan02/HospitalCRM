using Application.Interfaces.IServices.ApiBase;
using Application.Interfaces.IServices.Sms;
using Application.Models.Request.Sms;
using Application.Models.Response.Sms;
using Domain.Exceptions;

namespace Infrastructure.Sms;

public class SmsService : ISmsService
{

    private readonly IApiBaseService _apiBaseService;
    public const string PartnerApiName = "PartnerAPI";
    public const string SmsApiName = "SmsAPI";

    public SmsService(IApiBaseService apiBaseService)
    {
        _apiBaseService = apiBaseService;
    }


    public async Task SendSms(string text, string msisdn, string partnerId, long transactionId)
    {
        var partner = await _apiBaseService.GetAsync<ReceivingPartner>(partnerId, PartnerApiName);
        if (msisdn == null)
        {
            throw new BadRequestException("msisdn is null");
        }
        var smsRecord = new SmsRecord()
        {
            TransactionId = transactionId,
            Msisdn = msisdn,
            ShortNumber = "TEST DSC",
            SmsText = text,
            SmsDateTime = DateTime.Now,
            PublicKey = "test1234"
        };
        smsRecord.GenerateHashedKey(partner.Result.InternalResult.PrivateKey);
        var smsReq = new SendSmsReq()
        {
            SmsRecords = new List<SmsRecord>()
            {
                smsRecord
            }
        };

        var smsRes = await _apiBaseService.PostResourceAsync<SendSmsReq, SendSmsRes>(partnerId, smsReq, SmsApiName);

        if (smsRes.Responses.FirstOrDefault().ResultCode != 0)
        {
            throw new ThirdPartyServiceException(smsRes.Responses[0].ResultDescription);
        }
    }
}