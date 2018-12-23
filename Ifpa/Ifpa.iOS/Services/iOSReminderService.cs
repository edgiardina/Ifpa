using EventKit;
using Foundation;
using Ifpa.Interfaces;
using Ifpa.iOS.Services;
using Ifpa.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(iOSReminderService))]
namespace Ifpa.iOS.Services
{
    public class iOSReminderService : IReminderService
    {
        protected EKEventStore eventStore = new EKEventStore();

        public async Task<bool> CreateReminder(CalendarDetailViewModel calendarDetail)
        {
            var granted = await eventStore.RequestAccessAsync(EKEntityType.Event);//, (bool granted, NSError e) =>
            if (granted.Item1)
            {
                EKEvent newEvent = EKEvent.FromStore(eventStore);
                newEvent.StartDate = DateTimeToNSDate(calendarDetail.StartDate);
                newEvent.EndDate = DateTimeToNSDate(calendarDetail.EndDate);
                newEvent.Title = calendarDetail.TournamentName;
                newEvent.Notes = calendarDetail.Details;
                newEvent.Calendar = eventStore.DefaultCalendarForNewEvents;
                //TODO: we don't get start/end time for these so fix so its not all day
                newEvent.AllDay = true;
                newEvent.Location = $"{calendarDetail.Address1} {calendarDetail.City} {calendarDetail.State}";
                NSError e;
                eventStore.SaveEvent(newEvent, EKSpan.ThisEvent, out e);
                return true;
            }
            else
            {               
                return false;
            }
        }

        private DateTime NSDateToDateTime(NSDate date)
        {
            double secs = date.SecondsSinceReferenceDate;
            if (secs < -63113904000)
                return DateTime.MinValue;
            if (secs > 252423993599)
                return DateTime.MaxValue;
            return (DateTime)date;
        }

        private NSDate DateTimeToNSDate(DateTime date)
        {
            if (date.Kind == DateTimeKind.Unspecified)
                date = DateTime.SpecifyKind(date, DateTimeKind.Local);
            return (NSDate)date;
        }
    }
}