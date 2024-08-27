namespace Domain.Exceptions;

public class ThirdPartyServiceException : ApplicationException
{
    public ThirdPartyServiceException(string message) : base(message) { }
}