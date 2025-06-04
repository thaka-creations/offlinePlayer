using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace tplayer.Services
{
    public class ApiErrorResponse
    {
        public string Message { get; set; }
    }

    public class ApiErrorWrapper
    {
        public ApiErrorResponse Message { get; set; }
    }

    public class BaseHttpService
    {
        protected readonly HttpClient _httpClient;
        protected readonly JsonSerializerOptions _jsonOptions;

        public BaseHttpService()
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(15)
            };
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true // Makes JSON logging more readable
            };
        }

        private Uri EnsureValidUrl(string url)
        {
            try
            {
                return new Uri(url);
            }
            catch (UriFormatException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Invalid URL format: {url}");
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                throw new ArgumentException($"Invalid URL format: {url}", nameof(url), ex);
            }
        }

        protected async Task<T> PostAsync<T, TRequest>(string endpoint, TRequest data)
        {
            try
            {
                // Validate URL
                var uri = EnsureValidUrl(endpoint);
                System.Diagnostics.Debug.WriteLine($"Sending POST request to: {uri}");

                // Log request data
                var requestJson = JsonSerializer.Serialize(data, _jsonOptions);
                System.Diagnostics.Debug.WriteLine($"Request data: {requestJson}");

                var jsonContent = new StringContent(
                    requestJson,
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(uri, jsonContent);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Log the raw response
                System.Diagnostics.Debug.WriteLine($"Response status: {response.StatusCode}");
                System.Diagnostics.Debug.WriteLine($"Raw response: {jsonResponse}");

                if (!response.IsSuccessStatusCode)
                {
                    try
                    {
                        var errorWrapper = JsonSerializer.Deserialize<ApiErrorWrapper>(jsonResponse, _jsonOptions);
                        var errorMessage = errorWrapper?.Message?.Message ?? "An unknown error occurred";
                        System.Diagnostics.Debug.WriteLine($"Error message: {errorMessage}");
                        throw new HttpRequestException(errorMessage);
                    }
                    catch (JsonException ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Failed to parse error response: {ex.Message}");
                        throw new HttpRequestException($"Server error: {response.StatusCode}");
                    }
                }

                try
                {
                    var result = JsonSerializer.Deserialize<T>(jsonResponse, _jsonOptions);
                    System.Diagnostics.Debug.WriteLine($"Deserialized response: {JsonSerializer.Serialize(result, _jsonOptions)}");
                    return result;
                }
                catch (JsonException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to deserialize success response: {ex.Message}");
                    throw new HttpRequestException("Failed to parse server response", ex);
                }
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException)
            {
                System.Diagnostics.Debug.WriteLine($"Connection error: {ex.Message}");
                throw new HttpRequestException("Cannot connect to the server. Please check your internet connection and try again.", ex);
            }
            catch (TaskCanceledException)
            {
                System.Diagnostics.Debug.WriteLine("Request timed out");
                throw new HttpRequestException("The request timed out. Please check your connection and try again.");
            }
        }

        protected async Task<T> GetAsync<T>(string endpoint)
        {
            try
            {
                var uri = EnsureValidUrl(endpoint);
                System.Diagnostics.Debug.WriteLine($"Sending GET request to: {uri}");

                var response = await _httpClient.GetAsync(uri);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine($"Response status: {response.StatusCode}");
                System.Diagnostics.Debug.WriteLine($"Raw response: {jsonResponse}");

                if (!response.IsSuccessStatusCode)
                {
                    try
                    {
                        var errorWrapper = JsonSerializer.Deserialize<ApiErrorWrapper>(jsonResponse, _jsonOptions);
                        var errorMessage = errorWrapper?.Message?.Message ?? "An unknown error occurred";
                        System.Diagnostics.Debug.WriteLine($"Error message: {errorMessage}");
                        throw new HttpRequestException(errorMessage);
                    }
                    catch (JsonException ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Failed to parse error response: {ex.Message}");
                        throw new HttpRequestException($"Server error: {response.StatusCode}");
                    }
                }

                try
                {
                    var result = JsonSerializer.Deserialize<T>(jsonResponse, _jsonOptions);
                    System.Diagnostics.Debug.WriteLine($"Deserialized response: {JsonSerializer.Serialize(result, _jsonOptions)}");
                    return result;
                }
                catch (JsonException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to deserialize success response: {ex.Message}");
                    throw new HttpRequestException("Failed to parse server response", ex);
                }
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException)
            {
                System.Diagnostics.Debug.WriteLine($"Connection error: {ex.Message}");
                throw new HttpRequestException("Cannot connect to the server. Please check your internet connection and try again.", ex);
            }
            catch (TaskCanceledException)
            {
                System.Diagnostics.Debug.WriteLine("Request timed out");
                throw new HttpRequestException("The request timed out. Please check your connection and try again.");
            }
        }

        public void SetAuthToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                System.Diagnostics.Debug.WriteLine("Warning: Setting null or empty auth token");
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            System.Diagnostics.Debug.WriteLine("Auth token set successfully");
        }

        public void ClearAuthToken()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            System.Diagnostics.Debug.WriteLine("Auth token cleared");
        }

        protected async Task<bool> IsServerReachableAsync(string url)
        {
            try
            {
                var uri = EnsureValidUrl(url);
                System.Diagnostics.Debug.WriteLine($"Checking server reachability: {uri}");

                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5);
                var response = await client.GetAsync(uri);
                
                var isReachable = response.IsSuccessStatusCode;
                System.Diagnostics.Debug.WriteLine($"Server reachable: {isReachable}, Status: {response.StatusCode}");
                return isReachable;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Server unreachable: {ex.Message}");
                return false;
            }
        }
    }
} 