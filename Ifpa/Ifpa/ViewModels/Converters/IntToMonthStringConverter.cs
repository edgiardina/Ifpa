using System;
using System.Globalization;
using Xamarin.Forms;

namespace Ifpa.ViewModels.Converters
{
    public class IntToMonthStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new DateTime(DateTime.Now.Year, (int)value, 1).ToString("MMMM", CultureInfo.InvariantCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new MissingMethodException("Method Not implemented");
        }
    }
}
