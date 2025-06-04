using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using tplayer.Services;

namespace tplayer.ViewModel
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly AuthService _authService;
        
        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        private bool isPasswordVisible;

        public LoginViewModel()
        {
            _authService = new AuthService(new SecureStorageService());
            IsPasswordVisible = false;
        }

        [RelayCommand]
        void TogglePasswordVisibility()
        {
            IsPasswordVisible = !IsPasswordVisible;
        }

        [RelayCommand]
        async Task SignUp()
        {
            await Shell.Current.GoToAsync(nameof(RegisterPage));
        }

        [RelayCommand]
        async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter both username and password.";
                return;
            }

            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                
                await _authService.LoginAsync(Username, Password);

                // Clear sensitive data
                Username = string.Empty;
                Password = string.Empty;

                await Shell.Current.GoToAsync(nameof(MainPage));
            }
            catch (HttpRequestException ex)
            {
                ErrorMessage = ex.Message;
                System.Diagnostics.Debug.WriteLine($"Login error: {ex.Message}");
                
                // Show alert for connection errors
                if (ex.Message.Contains("Cannot connect") || ex.Message.Contains("not reachable"))
                {
                    await Shell.Current.DisplayAlert(
                        "Connection Error",
                        "Unable to connect to the server",
                        "OK"
                    );
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "An unexpected error occurred. Please try again.";
                System.Diagnostics.Debug.WriteLine($"Unexpected error: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        async Task ForgotPassword()
        {
            await Shell.Current.GoToAsync(nameof(ForgotPasswordPage));
        }
    }
}
