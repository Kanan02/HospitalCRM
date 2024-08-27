namespace Application.Models.Response.Sms;

public class VerifyOtpReq
{
    public int OtpCode { get; set; }
    public string Msisdn { get; set; }
    public string ServiceName { get; set; }
}