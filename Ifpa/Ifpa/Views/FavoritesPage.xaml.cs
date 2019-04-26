using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Ifpa.ViewModels;
using PinballApi.Models.WPPR.v1.Players;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoritesPage : ContentPage
    {
        FavoritesViewModel viewModel;

        public FavoritesPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new FavoritesViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var player = args.SelectedItem as Player;
            if (player == null)
                return;

            await Navigation.PushAsync(new PlayerDetailPage(new PlayerDetailViewModel(int.Parse(player.PlayerId))));

            // Manually deselect item.
            PlayersListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            viewModel.LoadItemsCommand.Execute(null);

            base.OnAppearing();
        }
    }
}