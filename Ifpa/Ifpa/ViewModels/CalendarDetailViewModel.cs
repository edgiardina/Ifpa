using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;

namespace Ifpa.ViewModels
{
    public class CalendarDetailViewModel : BaseViewModel
    {
        public string PrivateFlag { get; set; }

        public string Details { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public string DirectorName { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime StartDate { get; set; }

        public string TournamentDuration => $"{StartDate.ToString("d")}" + (StartDate != EndDate ? $" - {EndDate.ToString("d")}" : string.Empty );
        
        public string Website { get; set; }

        public string Zipcode { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Address2 { get; set; }

        public string Address1 { get; set; }

        public string TournamentName { get; set; }

        public int TournamentId { get; set; }
        
        public string CountryName { get; set; }

        public string Location => $"{Address1} {Address2} {City}{(!string.IsNullOrEmpty(City) && !string.IsNullOrEmpty(State) ? "," : string.Empty)} {State} {CountryName}".Trim().Replace("  ", " ");

        public Command LoadItemsCommand { get; set; }

        public int CalendarId { get; set; }

        public CalendarDetailViewModel(int calendarId)
        {
            this.CalendarId = calendarId;
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {                
                var items = await PinballRankingApi.GetCalendarById(CalendarId);

                var calendarEntry = items.Calendar.FirstOrDefault();
                Title = calendarEntry.TournamentName;
                Website = calendarEntry.Website;
                City = calendarEntry.City;
                DirectorName = calendarEntry.DirectorName;
                TournamentName = calendarEntry.TournamentName;
                Address1 = calendarEntry.Address1;
                Address2 = calendarEntry.Address2;
                City = calendarEntry.City;
                State = calendarEntry.State;
                CountryName = calendarEntry.CountryName;
                Details = calendarEntry.Details;
                StartDate = calendarEntry.StartDate;
                EndDate = calendarEntry.EndDate;
                TournamentId = calendarEntry.TournamentId;

                OnPropertyChanged(null);
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
