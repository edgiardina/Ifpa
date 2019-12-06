using System;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace Ifpa.ViewModels.Converters
{
    public class AlternatingRowListViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) return Color.White;
            return ((ListView)parameter).ItemsSource.Cast<object>().ToList().IndexOf(value) % 2 == 0 ? Color.White : Color.FromHex("#F1F1F1");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}