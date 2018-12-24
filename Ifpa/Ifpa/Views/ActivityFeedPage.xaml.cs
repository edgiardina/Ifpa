using Ifpa.Models;
using Ifpa.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivityFeedPage : ContentPage
    {
        private ActivityFeedViewModel activityFeedViewModel;

        public ActivityFeedPage()
        {
            InitializeComponent();

            BindingContext = activityFeedViewModel = new ActivityFeedViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (activityFeedViewModel.ActivityFeedItems.Count == 0)
                activityFeedViewModel.LoadItemsCommand.Execute(null);
        }

        private async void ActivityFeedListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var item = (ActivityFeedItem)e.SelectedItem;
                item.HasBeenSeen = true;
                await App.ActivityFeed.UpdateActivityFeedRecord(item);

                if (item.ActivityType == ActivityFeedType.TournamentResult)
                {
                    await Navigation.PushAsync(new TournamentResultsPage(new TournamentResultsViewModel(item.RecordID.Value)));
                }

                ActivityFeedListView.SelectedItem = null;
            }
        }
    }
}
