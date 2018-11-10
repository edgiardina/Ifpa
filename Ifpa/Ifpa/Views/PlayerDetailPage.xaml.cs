using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Ifpa.ViewModels;
using System;
using System.Threading.Tasks;

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
            BindingContext = this.viewModel = new PlayerDetailViewModel(0);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (LoadMyStats)
            {
                if (Application.Current.Properties.ContainsKey("PlayerId"))
                {
                    try
                    {
                        var id = Application.Current.Properties["PlayerId"] as string;
                        viewModel.PlayerId = int.Parse(id);                                               
                    }
                    catch(Exception ex)
                    {
                        await Navigation.PushModalAsync(new NavigationPage(new ConfigureMyStatsPage()));
                    }
                }
                else
                {
                    await Navigation.PushModalAsync(new NavigationPage(new ConfigureMyStatsPage()));
                }
            }

            viewModel.LoadItemsCommand.Execute(null);
        }

        private async void TournamentResults_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PlayerResultsPage(new PlayerResultsViewModel(viewModel.PlayerId)));
        }

        private async void Pvp_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PlayerVersusPlayerPage(new PlayerVersusPlayerViewModel(viewModel.PlayerId)));
        }

        private async Task Refresh_Clicked(object sender, EventArgs e)
        {
            RefreshIndicator.IsVisible = true;
            viewModel.LoadItemsCommand.Execute(null);
            await RefreshIndicator.RotateTo(360, 1250);
            RefreshIndicator.Rotation = 0;
            RefreshIndicator.IsVisible = false;
        }
    }
}