using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Application.Models.Request.Sms;

public class SendSmsReq
{
    [JsonProperty("smsRecords")]
    public List<SmsRecord> SmsRecords { get; set; }
}
public class SmsRecord
{

    [JsonProperty("requestType")]
    public string RequestType { get; set; } = "sms";
    [JsonProperty("transactionId")]
    public long TransactionId { get; set; }
    [JsonProperty("msisdn")]
    public string Msisdn { get; set; }
    [JsonProperty("shortNumber")]
    public string ShortNumber { get; set; }
    [JsonProperty("requestId")]
    public int RequestId { get; set; } = -1;
    [JsonProperty("smsText")]
    public string SmsText { get; set; }
    [JsonProperty("smsDateTime")]
    public DateTime SmsDateTime { get; set; }
    [JsonProperty("publicKey")]
    public string PublicKey { get; set; }
    [JsonProperty("hashedKey")]
    public string HashedKey { get; set; }


    public void GenerateHashedKey(string privateKey)
    {
        string dataToHash = TransactionId.ToString() + PublicKey + privateKey;
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(dataToHash);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }
            HashedKey = sb.ToString();
        }
    }
}