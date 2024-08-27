namespace Application.Interfaces.IServices.Sms;

public interface ISmsService
{
    Task SendSms(string text, string msisdn, string partnerId, long transactionId);
}