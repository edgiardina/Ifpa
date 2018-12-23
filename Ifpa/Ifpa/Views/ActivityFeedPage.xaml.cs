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

    }
}
