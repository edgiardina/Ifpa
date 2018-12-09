using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Ifpa.ViewModels;
using PinballApi.Models.WPPR.Rankings;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RankingsPage : ContentPage
    {
        RankingsViewModel viewModel;

        public RankingsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new RankingsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var player = args.SelectedItem as Ranking;
            if (player == null)
                return;

            await Navigation.PushAsync(new PlayerDetailPage(new PlayerDetailViewModel(player.PlayerId)));

            // Manually deselect item.
            PlayersListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Players.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        private void Picker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void Stepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            //todo: remember these values
            viewModel.LoadItemsCommand.Execute(null);
        }

    }
}