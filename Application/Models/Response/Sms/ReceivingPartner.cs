using Newtonsoft.Json;

namespace Application.Models.Response.Sms;

public class ReceivingPartner
{
    public string Message { get; set; }
    [JsonProperty("result")]
    public ExternalResult Result { get; set; }
}

public class ExternalResult
{
    public object Version { get; set; }
    public int StatusCode { get; set; }
    public object Message { get; set; }
    public object IsError { get; set; }
    public object ResponseException { get; set; }
    [JsonProperty("result")]
    public InternalResult InternalResult { get; set; }
}

public class InternalResult
{
    public string PartnerId { get; set; }
    public string ListeningUrl { get; set; }
    public string PrivateKey { get; set; }
    public string LogFolder { get; set; }
    public string[] ClientIps { get; set; }
    public IDictionary<string, List<string>> ShortNumbers { get; set; }
    public IDictionary<string, int> LastCheckedTransactionIds { get; set; }
    public string BsonObjectId { get; set; }
    public string AddedAtUtc { get; set; }
}
