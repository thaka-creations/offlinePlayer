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
            // Check if server is reachable first
            if (!await IsServerReachableAsync(ApiConfig.BaseUrl))
            {
                throw new HttpRequestException("Server is not reachable. Please check if the server is running and try again.");
            }

            var loginData = new { username, password };
            var response = await PostAsync<AuthResponseWrapper, object>(ApiConfig.Endpoints.Login, loginData);
            
            if (response?.Message != null)
            {
                await _secureStorage.StoreAuthDataAsync(response.Message);
                SetAuthToken(response.Message.AccessToken);
                return response.Message;
            }

            throw new HttpRequestException("Invalid response from server");
        }

        public async Task LogoutAsync()
        {
            await _secureStorage.ClearAuthDataAsync();
            ClearAuthToken();
        }

        public async Task<bool> InitializeAuthenticationAsync()
        {
            var authData = await _secureStorage.GetAuthDataAsync();
            if (authData != null)
            {
                SetAuthToken(authData.AccessToken);
                return true;
            }
            return false;
        }
    }
} 