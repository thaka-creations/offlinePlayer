using System.Globalization;

namespace tplayer.Converters
{
    public class BoolToPasswordVisibilityDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isVisible)
            {
                return isVisible ? "Hide password" : "Show password";
            }
            return "Show password";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 