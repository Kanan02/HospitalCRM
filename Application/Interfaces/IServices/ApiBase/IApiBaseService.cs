namespace Application.Interfaces.IServices.ApiBase;

public interface IApiBaseService
{
    Task<TResponse> PutResourceAsync<TRequest, TResponse>(string resourcePath, TRequest request,
        string clientName);

    Task<TResponse> PostResourceAsync<TRequest, TResponse>(string resourcePath, TRequest request,
        string clientName, Dictionary<string, string> extraHeaders = null);

    Task<TResponse> GetAsync<TResponse>(string url, string clientName);
}