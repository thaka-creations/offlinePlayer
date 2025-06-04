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

        public LoginViewModel()
        {
            _authService = new AuthService(new SecureStorageService());
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
                await Shell.Current.DisplayAlert("Error", "Please enter both username and password.", "OK");
                return;
            }

            try
            {
                IsLoading = true;
                await _authService.LoginAsync(Username, Password);

                // Clear sensitive data
                Username = string.Empty;
                Password = string.Empty;

                await Shell.Current.GoToAsync(nameof(MainPage));
            }
            catch (HttpRequestException ex)
            {
                await Shell.Current.DisplayAlert("Error", "Login failed. Please check your credentials.", "OK");
                System.Diagnostics.Debug.WriteLine($"Login error: {ex.Message}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "An unexpected error occurred.", "OK");
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
