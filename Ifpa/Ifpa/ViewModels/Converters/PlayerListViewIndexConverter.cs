using PinballApi.Extensions;
using System;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace Ifpa.ViewModels.Converters
{
    public class PlayerListViewIndexConverter : BindableObject, IValueConverter
    {

        public int StartingRank
        {
            get
            {
                return (int)GetValue(StartingRankProperty);
            }
            set
            {
                SetValue(StartingRankProperty, value);
            }
        }

        public ListView ListViewItem
        {
            get
            {
                return (ListView)GetValue(ListViewItemProperty);
            }
            set
            {
                SetValue(ListViewItemProperty, value);
            }
        }

        public static BindableProperty StartingRankProperty =
                    BindableProperty.Create(nameof(StartingRank), typeof(int), typeof(int), 1);

        public static BindableProperty ListViewItemProperty =
            BindableProperty.Create(nameof(ListViewItem), typeof(ListView), typeof(ListView));


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Color.White;
            return (ListViewItem.ItemsSource.Cast<object>().ToList().IndexOf(value) + StartingRank).OrdinalSuffix();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
