using Ifpa.ViewModels;
using PinballApi.Models.WPPR.v2.Players;
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
            
            var pvp = e.Item as PlayerVersusRecord;
            if (pvp == null)
                return;

            await Navigation.PushAsync(new PlayerVersusPlayerDetailPage (new PlayerVersusPlayerDetailViewModel(viewModel.PlayerId, pvp.PlayerId)));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.AllResults.Count == 0)
                viewModel.LoadAllItemsCommand.Execute(null);
        }

        private async void InfoButton_Clicked(object sender, System.EventArgs e)
        {
            string action = await DisplayActionSheet("PVP Type", null, null, "All", "Top 250");

            if(action == "All")
            {
                viewModel.LoadAllItemsCommand.Execute(null);
                MyListView.SetBinding(ListView.ItemsSourceProperty, "AllResults"); 
            }
            else
            {
                viewModel.LoadEliteItemsCommand.Execute(null);
                MyListView.SetBinding(ListView.ItemsSourceProperty, "EliteResults");
            }
        }
    }
}
