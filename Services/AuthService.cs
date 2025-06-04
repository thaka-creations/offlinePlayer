using System.Diagnostics;
using tplayer.Config;
using tplayer.Models;

namespace tplayer.Services
{
    public class AuthService : BaseHttpService
    {
        private readonly SecureStorageService _secureStorage;

        public AuthService(SecureStorageService secureStorage) : base()
        {
            _secureStorage = secureStorage;
        }

        public async Task<AuthResponse> LoginAsync(string username, string password)
        {
            try
            {
                Debug.WriteLine($"Attempting login for user: {username}");
                Debug.WriteLine($"Using login endpoint: {ApiConfig.Endpoints.Login}");

                // Check if server is reachable first
                if (!await IsServerReachableAsync(ApiConfig.BaseUrl))
                {
                    throw new HttpRequestException("Server is not reachable. Please check if the server is running and try again.");
                }

                var loginData = new { username, password };
                Debug.WriteLine("Sending login request...");
                
                var response = await PostAsync<AuthResponseWrapper, object>(ApiConfig.Endpoints.Login, loginData);
                
                if (response?.Message != null)
                {
                    Debug.WriteLine($"Login successful for user: {response.Message.Username}");
                    await _secureStorage.StoreAuthDataAsync(response.Message);
                    SetAuthToken(response.Message.AccessToken);
                    return response.Message;
                }

                Debug.WriteLine("Login response was empty or invalid");
                throw new HttpRequestException("Invalid response from server");
            }
            catch (ArgumentException ex)
            {
                Debug.WriteLine($"URL Construction Error: {ex.Message}");
                throw new HttpRequestException("Invalid API configuration. Please contact support.", ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Login Error: {ex.GetType().Name} - {ex.Message}");
                throw;
            }
        }

        public async Task LogoutAsync()
        {
            try
            {
                Debug.WriteLine("Performing logout...");
                await _secureStorage.ClearAuthDataAsync();
                ClearAuthToken();
                Debug.WriteLine("Logout completed successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Logout Error: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> InitializeAuthenticationAsync()
        {
            try
            {
                Debug.WriteLine("Initializing authentication...");
                var authData = await _secureStorage.GetAuthDataAsync();
                if (authData != null)
                {
                    Debug.WriteLine($"Found stored auth data for user: {authData.Username}");
                    SetAuthToken(authData.AccessToken);
                    return true;
                }
                Debug.WriteLine("No stored auth data found");
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Authentication Initialization Error: {ex.Message}");
                return false;
            }
        }
    }
} 