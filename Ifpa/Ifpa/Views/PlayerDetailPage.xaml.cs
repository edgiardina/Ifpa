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
                //Hide Star on our own page
                if(ToolbarItems.Count == 2)
                    ToolbarItems.RemoveAt(1);

                if (Application.Current.Properties.ContainsKey("PlayerId"))
                {
                    try
                    {
                        viewModel.PlayerId = (int)Application.Current.Properties["PlayerId"];
                    }
                    catch (Exception ex)
                    {
                        await RedirectUserToPlayerSearch();
                    }
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

        private async Task StarButton_Clicked(object sender, EventArgs e)
        {
            if (!Application.Current.Properties.ContainsKey("PlayerId"))
            {
                await ChangePlayerAndRedirect();
            }
            else
            {
                var result = await DisplayAlert("Caution", "You have already configured your Stats page, do you wish to change your Stats to this player?", "OK", "Cancel");
                if(result)
                {
                    await ChangePlayerAndRedirect();
                }
            }
        }

        private async Task ChangePlayerAndRedirect()
        {
            Application.Current.Properties["PlayerId"] = viewModel.PlayerId;
            await DisplayAlert("Congratulations", "You have now configured your Stats page!", "OK");
            var masterPage = this.Parent.Parent as TabbedPage;
            masterPage.CurrentPage = masterPage.Children[2];
        }

        private async Task RedirectUserToPlayerSearch()
        {
            await DisplayAlert("Configure your Stats", "Looks like you haven't configured your 'My Stats' page. Use the Player Search to find your Player, and press the Star to configure your Stats", "OK");
            var masterPage = this.Parent.Parent as TabbedPage;
            masterPage.CurrentPage = masterPage.Children[1];
        }
    }
}