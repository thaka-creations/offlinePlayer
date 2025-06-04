using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace tplayer.ViewModel
{
    public partial class HomeViewModel : ObservableObject
    {
        [ObservableProperty]
        private string activationKey;

        [RelayCommand]
        async Task OpenPlayer()
        {
            try
            {
                await Shell.Current.GoToAsync($"//{nameof(PlayerPage)}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Could not open player. Please try again.", "OK");
                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");
            }
        }

        [RelayCommand]
        async Task MyAccount()
        {
            try
            {
                await Shell.Current.GoToAsync($"//{nameof(AccountPage)}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Could not open account page. Please try again.", "OK");
                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");
            }
        }

        [RelayCommand]
        async Task ShowActivationDialog()
        {
            string result = await Shell.Current.DisplayPromptAsync(
                "Activate Key",
                "Enter your activation key:",
                "Activate",
                "Cancel",
                placeholder: "Enter key here...");

            if (!string.IsNullOrWhiteSpace(result))
            {
                await ActivateKey(result);
            }
        }

        private async Task ActivateKey(string key)
        {
            try
            {
                // TODO: Implement key activation logic here
                bool isValid = await ValidateActivationKey(key);
                
                if (isValid)
                {
                    await Shell.Current.DisplayAlert("Success", "Key activated successfully!", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Invalid activation key. Please try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Could not activate key. Please try again.", "OK");
                System.Diagnostics.Debug.WriteLine($"Activation error: {ex.Message}");
            }
        }

        private async Task<bool> ValidateActivationKey(string key)
        {
            // TODO: Implement actual key validation logic
            await Task.Delay(1000); // Simulate network request
            return !string.IsNullOrEmpty(key) && key.Length > 8;
        }
    }
}
