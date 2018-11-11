using Ifpa.ViewModels;
using PinballApi.Models.WPPR.Tournaments;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerVersusPlayerPage : ContentPage
    {
        PlayerVersusPlayerViewModel viewModel;

        public PlayerVersusPlayerPage(PlayerVersusPlayerViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            
            var tournament = e.Item as Result;
            if (tournament == null)
                return;

            //await Navigation.PushAsync(new PlayerDetailPage(new PlayerDetailViewModel(tournament.PlayerId)));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Results.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}
