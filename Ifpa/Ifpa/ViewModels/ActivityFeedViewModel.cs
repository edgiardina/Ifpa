using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Ifpa.Models;

namespace Ifpa.ViewModels
{
    public class ActivityFeedViewModel : BaseViewModel
    {
        public ObservableCollection<ActivityFeedItem> ActivityFeedItems { get; set; }
        public Command LoadItemsCommand { get; set; }
                
        public ActivityFeedViewModel()
        {
            Title = "Activity Feed";
            ActivityFeedItems = new ObservableCollection<ActivityFeedItem>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var activityFeedItems = await Settings.LocalDatabase.GetActivityFeedRecords();
                ActivityFeedItems.Clear();
                foreach(var item in activityFeedItems)
                {
                    ActivityFeedItems.Add(item);
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