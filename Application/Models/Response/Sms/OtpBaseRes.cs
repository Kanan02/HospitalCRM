namespace Application.Models.Response.Sms;

public class OtpBaseRes<T>
{
    public bool Success { get; set; }
    public List<Error> Errors { get; set; }
    public T? Value { get; set; }
}

public class Error
{
    public string ErrorMessage { get; set; }
}