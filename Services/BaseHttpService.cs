using System.Net.Sockets;
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
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(15) // Set timeout to 15 seconds
            };
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        protected async Task<T> PostAsync<T, TRequest>(string endpoint, TRequest data)
        {
            try
            {
                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(data),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(endpoint, jsonContent);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Server returned error: {response.StatusCode} - {jsonResponse}");
                }

                return JsonSerializer.Deserialize<T>(jsonResponse, _jsonOptions);
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException)
            {
                throw new HttpRequestException("Cannot connect to the server. Please check your internet connection and try again.", ex);
            }
            catch (TaskCanceledException)
            {
                throw new HttpRequestException("The request timed out. Please check your connection and try again.");
            }
        }

        protected async Task<T> GetAsync<T>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Server returned error: {response.StatusCode} - {jsonResponse}");
                }

                return JsonSerializer.Deserialize<T>(jsonResponse, _jsonOptions);
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException)
            {
                throw new HttpRequestException("Cannot connect to the server. Please check your internet connection and try again.", ex);
            }
            catch (TaskCanceledException)
            {
                throw new HttpRequestException("The request timed out. Please check your connection and try again.");
            }
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

        // Check if server is reachable
        protected async Task<bool> IsServerReachableAsync(string url)
        {
            try
            {
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5);
                var response = await client.GetAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
} 