using System.Globalization;
using Microsoft.Maui.Controls;

namespace tplayer.Converters
{
    public class BoolToPasswordVisibilityIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isVisible)
            {
                return isVisible ? "🔓" : "🔒";  // Using Unicode emoji for better compatibility
            }
            return "🔒";  // Default to locked icon
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 