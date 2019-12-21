using Ifpa.Models;
using Ifpa.ViewModels;
using Syncfusion.SfCalendar.XForms;
using System;
using Xamarin.Forms;

namespace Ifpa.Views.Behaviors
{
    class CalendarBehavior : Behavior<ContentPage>
    {
        SfCalendar calendar;
        ContentPage page;

        protected override void OnAttachedTo(ContentPage bindable)
        {
            base.OnAttachedTo(bindable);
            calendar = bindable.FindByName<SfCalendar>("calendar");
            calendar.OnInlineLoaded += Calendar_OnInlineLoaded;
            page = bindable;
        }

        private void Calendar_OnInlineLoaded(object sender, InlineEventArgs e)
        {
            var stackLayout = new StackLayout();

            if (e.Appointments.Count > 0)
            {
                foreach (InlineCalendarItem appt in e.Appointments)
                {
                    Button button = new Button();
                    button.BackgroundColor = Color.Transparent;
                    button.TextColor = (Color)Application.Current.Resources["PrimaryTextColor"];
                    button.Text = appt.Subject;
                    button.CommandParameter = appt.CalendarId;
                    button.Clicked += Button_Clicked;
                    stackLayout.Children.Add(button);
                }
            }
            else
            {
                var label = new Button();
                label.Text = "No Tournaments";
                label.TextColor = Color.Black;
                stackLayout.Children.Add(label);
            }

            e.View = stackLayout;

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var calendarId = (int)button.CommandParameter;
            await page.Navigation.PushAsync(new CalendarDetailPage(new CalendarDetailViewModel(calendarId)));
        }
    }
}
