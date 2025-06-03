using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace tplayer.ViewModel
{
    public partial class LoginViewModel
    {
        [RelayCommand]
        async Task SignUp()
        {
            await Shell.Current.GoToAsync(nameof(RegisterPage));
        }

        [RelayCommand]
        async Task Login()
        {
            //TODO: Implement login logic
        }

        [RelayCommand]
        async Task ForgotPassword()
        {
            await Shell.Current.GoToAsync(nameof(ForgotPasswordPage));
        }
    }
}
