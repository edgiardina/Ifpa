using PinballApi.Models.WPPR.v2.Tournaments;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace Ifpa.ViewModels.Converters
{
    public class TournamentResultIncludedColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return (Color)Application.Current.Resources["TournamentResultIncludedRecord"];

            var tournamentResult = (TournamentResult)value;

            return
                tournamentResult.ExcludedFlag ? (Color)Application.Current.Resources["TournamentResultExcludedRecord"]
                    : (Color)Application.Current.Resources["TournamentResultIncludedRecord"]; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
