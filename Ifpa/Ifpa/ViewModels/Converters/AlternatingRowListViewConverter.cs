using System;
using System.Globalization;
using System.Linq;
using Microsoft.Maui;

namespace Ifpa.ViewModels.Converters
{
    public class AlternatingRowListViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) return Color.White;
            return 
                ((ListView)parameter).ItemsSource.Cast<object>().ToList().IndexOf(value) % 2 == 0 ?
                    (Color)Application.Current.Resources["BackgroundColor"] :
                    (Color)Application.Current.Resources["SecondaryBackgroundColor"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}