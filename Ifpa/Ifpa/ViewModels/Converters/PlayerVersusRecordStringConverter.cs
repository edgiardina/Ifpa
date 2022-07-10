using PinballApi.Models.WPPR.v2.Players;
using System;
using System.Globalization;
using Microsoft.Maui;

namespace Ifpa.ViewModels.Converters
{
    public class PlayerVersusRecordStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Colors.Transparent;

            var pvpRecord = (PlayerVersusRecord)value;

            return
                pvpRecord.WinCount > pvpRecord.LossCount ? (Color)Application.Current.Resources["WinningRecord"]
                    : pvpRecord.WinCount < pvpRecord.LossCount ? (Color)Application.Current.Resources["LosingRecord"]
                    : (Color)Application.Current.Resources["TieRecord"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
