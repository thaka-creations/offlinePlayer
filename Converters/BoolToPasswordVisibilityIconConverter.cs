using System.Globalization;

namespace tplayer.Converters
{
    public class BoolToPasswordVisibilityIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isVisible)
            {
                return isVisible ? "eye_slash.png" : "eye.png";
            }
            return "eye.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 