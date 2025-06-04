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
                await Shell.Current.DisplayAlert("Validation Error", "Please enter both username and password.", "OK");
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

                await Shell.Current.GoToAsync(nameof(RegisterPage));
            }
            catch (HttpRequestException ex)
            {
                // Display server errors as alerts
                await Shell.Current.DisplayAlert("Login Failed", ex.Message, "OK");
                System.Diagnostics.Debug.WriteLine($"Login error: {ex.Message}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "An unexpected error occurred. Please try again.", "OK");
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
