using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Spectator.Windows.Views
{
    class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ("invert".Equals(parameter))
            {
                return true.Equals(value) ? Visibility.Collapsed : Visibility.Visible;
            }
            return true.Equals(value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}