using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Provider;
using Ifpa.Droid.Services;
using Ifpa.Interfaces;
using Ifpa.ViewModels;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidReminderService))]
namespace Ifpa.Droid.Services
{
    public class AndroidReminderService : IReminderService
    {
        public async Task<bool> CreateReminder(CalendarDetailViewModel calendarDetail)
        {
            Intent intent = new Intent(Intent.ActionInsert);
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.Title, calendarDetail.TournamentName);
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.Description, calendarDetail.Details);            
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.Dtstart, new DateTimeOffset(calendarDetail.StartDate).ToUnixTimeMilliseconds());
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.Dtend, new DateTimeOffset(calendarDetail.EndDate).ToUnixTimeMilliseconds());

            intent.PutExtra(CalendarContract.EventsColumns.EventLocation, $"{calendarDetail.Address1} {calendarDetail.City} {calendarDetail.State}");
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.EventTimezone, "UTC");
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.EventEndTimezone, "UTC");
            intent.SetData(CalendarContract.Events.ContentUri);
            Forms.Context.StartActivity(intent);
            return true;
        }
    }
}