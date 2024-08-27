using System.Text.Json.Serialization;

namespace Application.Models.Request.Sms;

public class SendOtpReq
{
    public string Msisdn { get; set; }
    [JsonIgnore]
    public string? ServiceName { get; set; }
}