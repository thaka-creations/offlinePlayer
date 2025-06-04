using System.Text;
using System.Text.Json;

namespace tplayer.Services
{
    public class BaseHttpService
    {
        protected readonly HttpClient _httpClient;
        protected readonly JsonSerializerOptions _jsonOptions;

        public BaseHttpService()
        {
            _httpClient = new HttpClient();
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        protected async Task<T> PostAsync<T, TRequest>(string endpoint, TRequest data)
        {
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(data),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync(endpoint, jsonContent);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"API request failed: {response.StatusCode} - {jsonResponse}");
            }

            return JsonSerializer.Deserialize<T>(jsonResponse, _jsonOptions);
        }

        protected async Task<T> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"API request failed: {response.StatusCode} - {jsonResponse}");
            }

            return JsonSerializer.Deserialize<T>(jsonResponse, _jsonOptions);
        }

        // Add authorization header
        public void SetAuthToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        // Remove authorization header
        public void ClearAuthToken()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
} 