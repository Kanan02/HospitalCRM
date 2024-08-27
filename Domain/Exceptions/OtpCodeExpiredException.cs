namespace Domain.Exceptions;

public class OtpCodeExpiredException : ApplicationException
{
    public OtpCodeExpiredException(string message) : base(message)
    {

    }
}