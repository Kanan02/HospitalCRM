using Newtonsoft.Json;

namespace Application.Models.Response.Sms;

public class SendSmsRes
{
    [JsonProperty("responses")] public List<SmsInfo> Responses { get; set; }

}

public class SmsInfo
{
    [JsonProperty("resultCode")]
    public int ResultCode { get; set; }
    [JsonProperty("resultDescription")]
    public string ResultDescription { get; set; }
}