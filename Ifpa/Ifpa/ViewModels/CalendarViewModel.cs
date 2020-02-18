using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using PinballApi.Models.WPPR.v1.Calendar;
using System.Linq;
using Ifpa.Models;
using Syncfusion.SfCalendar.XForms;

namespace Ifpa.ViewModels
{
    public class CalendarViewModel : BaseViewModel
    {
        public ObservableCollection<CalendarDetails> CalendarDetails { get; set; }
        public CalendarEventCollectionRange InlineCalendarItems { get; set; }
        public Command LoadItemsCommand { get; set; }

        public CalendarViewModel()
        {
            Title = "Calendar";
            CalendarDetails = new ObservableCollection<CalendarDetails>();
            InlineCalendarItems = new CalendarEventCollectionRange();
        }

        public async Task ExecuteLoadItemsCommand(string address, int distance)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var sw = Stopwatch.StartNew();
                CalendarDetails.Clear();
                InlineCalendarItems.Clear();
                Console.WriteLine("Cleared collections in {0}", sw.ElapsedMilliseconds);

                var items = await PinballRankingApi.GetCalendarSearch(address, distance, DistanceUnit.Miles);

                Console.WriteLine("Api call completed at {0}", sw.ElapsedMilliseconds);
                if (items.Calendar.Any())
                {
                    foreach (var item in items.Calendar.OrderBy(n => n.EndDate))
                    {
                        CalendarDetails.Add(item);                        
                    }

                    InlineCalendarItems.AddRange(
                        items.Calendar.Where(item => item.EndDate - item.StartDate <= 5.Days())
                                      .Select(i => 
                                        new InlineCalendarItem
                                        {
                                            CalendarId = i.CalendarId,
                                            Subject = i.TournamentName,
                                            StartTime = i.StartDate.Date,
                                            EndTime = i.EndDate.Date,
                                            IsAllDay = true
                                        })
                    );
                }

                Console.WriteLine("Collections loaded at {0}", sw.ElapsedMilliseconds);
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