using Microsoft.Maui.Controls;

namespace tplayer.Behaviors
{
    public class DisplayErrorBehavior : Behavior<ContentPage>
    {
        public static readonly BindableProperty ErrorMessageProperty =
            BindableProperty.Create(nameof(ErrorMessage), typeof(string), typeof(DisplayErrorBehavior), null,
                propertyChanged: OnErrorMessageChanged);

        public string ErrorMessage
        {
            get => (string)GetValue(ErrorMessageProperty);
            set => SetValue(ErrorMessageProperty, value);
        }

        private static async void OnErrorMessageChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = (DisplayErrorBehavior)bindable;
            if (!string.IsNullOrEmpty((string)newValue))
            {
                await behavior.DisplayError((string)newValue);
            }
        }

        private async Task DisplayError(string message)
        {
            if (Application.Current?.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", message, "OK");
                ErrorMessage = null; // Reset the message
            }
        }
    }
} 