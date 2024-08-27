using Application.Interfaces.IServices.ApiBase;
using System.Text;

namespace Infrastructure.ApiBase;

public class ApiBaseService : IApiBaseService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ApiBaseService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }


    public async Task<TResponse> PutResourceAsync<TRequest, TResponse>(string resourcePath, TRequest request,
        string clientName)
    {
        var client = _httpClientFactory.CreateClient(clientName);

        var jsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(request);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        var response = await client.PutAsync(resourcePath, content);
        var jsonResponse = await response.Content.ReadAsStringAsync();

        response.EnsureSuccessStatusCode();

        var responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject<TResponse>(jsonResponse);

        return responseObject;
    }
    public async Task<TResponse> PostResourceAsync<TRequest, TResponse>(string resourcePath, TRequest request,
        string clientName, Dictionary<string, string> extraHeaders = null)
    {
        var client = _httpClientFactory.CreateClient(clientName);

        if (extraHeaders != null)
        {
            foreach (var header in extraHeaders)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        var jsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(request);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");


        var response = await client.PostAsync(resourcePath, content);
        var jsonResponse = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();
        var responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject<TResponse>(jsonResponse);

        return responseObject;
    }
    public async Task<TResponse> GetAsync<TResponse>(string url, string clientName)
    {
        // Set the authentication token if provided
        var client = _httpClientFactory.CreateClient(clientName);

        // Send the GET request and handle the response
        HttpResponseMessage response = await client.GetAsync(url);

        // Ensure the request was successful
        response.EnsureSuccessStatusCode();

        // Deserialize the response content to the desired type
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject<TResponse>(jsonResponse);

        return responseObject;
    }
}