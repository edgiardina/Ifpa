using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using PinballApi.Models.WPPR.Calendar;
using System.Threading.Tasks;

namespace Ifpa.ViewModels
{
    public class CalendarDetailViewModel : BaseViewModel
    {
        public CalendarEntry CalendarEntry { get; set; }
        public Command LoadItemsCommand { get; set; }

        public int CalendarId { get; set; }

        public CalendarDetailViewModel(int calendarId)
        {
            this.CalendarId = calendarId;
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {

        }



    }
}
