namespace Application.Models.Request.Security;

public class ResetPwdReq
{
    public string Msisdn { get; set; }
    public int OtpCode { get; set; }
    public string NewPassword { get; set; }
}