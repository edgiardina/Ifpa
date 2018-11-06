using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Ifpa.ViewModels;
using PinballApi.Models.WPPR.Players;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerDetailPage : ContentPage
    {
        PlayerDetailViewModel viewModel;

        bool LoadMyStats = false;

        public PlayerDetailPage(PlayerDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;            
        }

        public PlayerDetailPage()
        {
            InitializeComponent();

            LoadMyStats = true;

            viewModel = new PlayerDetailViewModel(new PlayerRecord { Player = new Player { PlayerId = "-1" }, PlayerStats = new PlayerStats { CurrentWpprRank = 1 } });
            BindingContext = viewModel;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (LoadMyStats)
            {
                if (Application.Current.Properties.ContainsKey("PlayerId"))
                {
                    var id = Application.Current.Properties["PlayerId"] as string;
                    var playerRecord = await viewModel.PinballRankingApi.GetPlayerRecord(int.Parse(id));

                    viewModel = new PlayerDetailViewModel(playerRecord);
                    BindingContext = viewModel;
                }
                else
                {
                    await Navigation.PushModalAsync(new NavigationPage(new ConfigureMyStatsPage()));
                }
            }
        }
    }
}