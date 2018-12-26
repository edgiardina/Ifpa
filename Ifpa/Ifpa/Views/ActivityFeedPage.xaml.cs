using Ifpa.Models;
using Ifpa.ViewModels;
using Plugin.Badge;
using System;
using System.ComponentModel;
using System.Linq;
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

                if (Device.RuntimePlatform == Device.iOS)
                {
                    var remainingUnreads = await App.ActivityFeed.GetUnreadActivityCount();
                    CrossBadge.Current.SetBadge(remainingUnreads);
                }

                ActivityFeedListView.SelectedItem = null;
                activityFeedViewModel.LoadItemsCommand.Execute(null);
            }
        }

        private async void ToolbarItem_Clicked(object sender, System.EventArgs e)
        {
            foreach (var i in activityFeedViewModel.ActivityFeedItems.Where(n => !n.HasBeenSeen))
            {
                i.HasBeenSeen = true;
                await App.ActivityFeed.UpdateActivityFeedRecord(i);
            }           
            
            if (Device.RuntimePlatform == Device.iOS)
            {
                CrossBadge.Current.ClearBadge();
            }
            
            activityFeedViewModel.LoadItemsCommand.Execute(null);
        }

        //Testing shim. 
        private async void ToolbarItem_Clicked_1(object sender, System.EventArgs e)
        {
            var newItem = new ActivityFeedItem
            {
                ActivityType = ActivityFeedType.TournamentResult,
                RecordID = 28089,
                CreatedDateTime = DateTime.Now,
                HasBeenSeen = false
            };
            await App.ActivityFeed.CreateActivityFeedRecord(newItem);            
            CrossBadge.Current.SetBadge(1);
            activityFeedViewModel.LoadItemsCommand.Execute(null);
        }
    }
}
