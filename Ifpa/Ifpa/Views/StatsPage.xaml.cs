using Ifpa.ViewModels;
using PinballApi.Models.WPPR.v1.Statistics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatsPage : ContentPage
    {
        private StatsViewModel viewModel;

        public StatsPage()
        {
            InitializeComponent();

            BindingContext = this.viewModel = new StatsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.LoadItemsCommand.Execute(null);
        }

        private async void PlayersListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var player = e.SelectedItem as PointsThisYearStat;
            if (player == null)
                return;

            await Navigation.PushAsync(new PlayerDetailPage(new PlayerDetailViewModel(player.PlayerId)));

            // Manually deselect item.
            PlayersListView.SelectedItem = null;
        }

        private async void MostEventsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var player = e.SelectedItem as MostEventsStat;
            if (player == null)
                return;

            await Navigation.PushAsync(new PlayerDetailPage(new PlayerDetailViewModel(player.PlayerId)));

            // Manually deselect item.
            MostEventsListView.SelectedItem = null;
        }
    }
}