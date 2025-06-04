using System.Text.Json;
using tplayer.Models;

namespace tplayer.Services
{
    public class SecureStorageService
    {
        private const string AUTH_KEY = "auth_data";
        
        public async Task StoreAuthDataAsync(AuthResponse authResponse)
        {
            try
            {
                var jsonString = JsonSerializer.Serialize(authResponse);
                await SecureStorage.Default.SetAsync(AUTH_KEY, jsonString);
            }
            catch (Exception ex)
            {
                // Log the error and rethrow
                System.Diagnostics.Debug.WriteLine($"Error storing auth data: {ex.Message}");
                throw;
            }
        }

        public async Task<AuthResponse> GetAuthDataAsync()
        {
            try
            {
                var jsonString = await SecureStorage.Default.GetAsync(AUTH_KEY);
                if (string.IsNullOrEmpty(jsonString))
                    return null;

                return JsonSerializer.Deserialize<AuthResponse>(jsonString);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error retrieving auth data: {ex.Message}");
                return null;
            }
        }

        public async Task ClearAuthDataAsync()
        {
            try
            {
                SecureStorage.Default.Remove(AUTH_KEY);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error clearing auth data: {ex.Message}");
                throw;
            }
        }

        public bool IsAuthenticated()
        {
            return !string.IsNullOrEmpty(SecureStorage.Default.GetAsync(AUTH_KEY).Result);
        }
    }
} 