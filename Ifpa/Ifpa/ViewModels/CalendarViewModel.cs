using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using PinballApi.Models.WPPR.Rankings;
using PinballApi.Models.WPPR.Calendar;
using System.Linq;

namespace Ifpa.ViewModels
{
    public class CalendarViewModel : BaseViewModel
    {
        public ObservableCollection<CalendarDetails> CalendarDetails { get; set; }
        public Command LoadItemsCommand { get; set; }

        public CalendarViewModel()
        {
            Title = "Calendar";
            CalendarDetails = new ObservableCollection<CalendarDetails>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand("Chicago, Illinois", 100));
        }

        public async Task ExecuteLoadItemsCommand(string address, int distance)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                CalendarDetails.Clear();
                var items = await PinballRankingApi.GetCalendarSearch(address, distance, DistanceUnit.Miles);

                foreach (var item in items.Calendar.OrderBy(n => n.EndDate))
                {
                    CalendarDetails.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}