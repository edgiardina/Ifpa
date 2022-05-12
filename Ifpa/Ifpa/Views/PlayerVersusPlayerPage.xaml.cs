using Ifpa.ViewModels;
using PinballApi.Models.WPPR.v2.Players;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ifpa.Views
{
    [QueryProperty("PlayerId", "playerId")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerVersusPlayerPage : ContentPage
    {
        PlayerVersusPlayerViewModel viewModel;

        public int PlayerId { get; set; }

        public PlayerVersusPlayerPage()
        {
            InitializeComponent();

            BindingContext = this.viewModel = App.GetViewModel<PlayerVersusPlayerViewModel>();
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {            
            var pvp = e.Item as PlayerVersusRecord;
            if (pvp == null)
                return;

            await Shell.Current.GoToAsync($"pvp-detail?playerId={viewModel.PlayerId}&comparePlayerId={pvp.PlayerId}");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.AllResults.Count == 0)
            {
                viewModel.PlayerId = PlayerId;
                viewModel.LoadAllItemsCommand.Execute(null);
            }
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
