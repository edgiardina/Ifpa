﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Ifpa.ViewModels;
using System;
using System.Threading.Tasks;
using System.Linq;
using Ifpa.Services;
using Ifpa.Models;
using Xamarin.Essentials;
using Syncfusion.SfChart.XForms;

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
            viewModel.IsBusy = true;
            BindingContext = this.viewModel = viewModel;
        }

        public PlayerDetailPage()
        {
            InitializeComponent();

            LoadMyStats = true;
            BindingContext = this.viewModel = new PlayerDetailViewModel();
            viewModel.IsBusy = true;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (LoadMyStats)
            {
                ToolbarItems.Remove(ToolbarItems.SingleOrDefault(n => n.Text == "Set to My Stats"));
                ToolbarItems.Remove(ToolbarItems.SingleOrDefault(n => n.Text == "Favorite"));

                if (Settings.HasConfiguredMyStats)
                {
                    try
                    {
                        viewModel.PlayerId = Settings.MyStatsPlayerId;
                    }
                    catch (Exception)
                    {
                        await RedirectUserToPlayerSearch();
                    }
                }

                viewModel.PostPlayerLoadCommand = new Command(async () => await PostPlayerLoad());
            }
            else
            {
                if (Settings.HasConfiguredMyStats)
                {
                    ToolbarItems.Remove(ToolbarItems.SingleOrDefault(n => n.Text == "Set to My Stats"));

                    //if player is in the existing favorites list, fill the heart icon.
                    if (await Settings.LocalDatabase.HasFavorite(viewModel.PlayerId))
                    {
                        SetCorrectFavoriteIcon(true);
                    }
                }
                else
                {
                    ToolbarItems.Remove(ToolbarItems.SingleOrDefault(n => n.Text == "Favorite"));
                }
                ToolbarItems.Remove(ToolbarItems.SingleOrDefault(n => n.Text == "Activity Feed"));
            }

            viewModel.LoadItemsCommand.Execute(null);

            //there's some sort of chart bug here so set the chart color manually again
            RankProgressChart.Title.TextColor = (Color)Application.Current.Resources["PrimaryTextColor"];
            RatingProgressChart.Title.TextColor = (Color)Application.Current.Resources["PrimaryTextColor"];
        }

        /// <summary>
        /// Do tasks we need the UI to be fully re-drawn for.
        /// </summary>
        /// <returns></returns>
        private async Task PostPlayerLoad()
        {
            var numOfUnread = await Settings.LocalDatabase.GetUnreadActivityCount();
            DependencyService.Get<IToolbarItemBadgeService>().SetBadge(this, ToolbarItems.SingleOrDefault(n => n.Text == "Activity Feed"), numOfUnread.ToString(), Color.Red, Color.White);
        }

        private async void TournamentResults_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PlayerResultsPage(new PlayerResultsViewModel(viewModel.PlayerId)));
        }

        private async void Pvp_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PlayerVersusPlayerPage(new PlayerVersusPlayerViewModel(viewModel.PlayerId)));
        }

        private async void StarButton_Clicked(object sender, EventArgs e)
        {
            if (!Settings.HasConfiguredMyStats)
            {
                await ChangePlayerAndRedirect();
            }
            else
            {
                var result = await DisplayAlert("Caution", "You have already configured your Stats page, do you wish to change your Stats to this player?", "OK", "Cancel");
                if (result)
                {
                    await ChangePlayerAndRedirect();
                }
            }
        }

        private async Task ChangePlayerAndRedirect()
        {
            await Settings.SetMyStatsPlayer(viewModel.PlayerId, viewModel.PlayerRecord.PlayerStats.CurrentWpprRank);

            await DisplayAlert("Congratulations", "You have now configured your Stats page!", "OK");
            var masterPage = this.Parent.Parent as TabbedPage;            
            masterPage.CurrentPage = masterPage.Children[2];
            await this.Navigation.PopToRootAsync();
        }

        private async Task RedirectUserToPlayerSearch()
        {
            await DisplayAlert("Configure your Stats", "Looks like you haven't configured your 'My Stats' page. Use the Player Search to find your Player, and press the Star to configure your Stats", "OK");
            var masterPage = this.Parent.Parent as TabbedPage;
            masterPage.CurrentPage = masterPage.Children[0];
        }

        private async void ActivityFeedButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ActivityFeedPage());
        }

        private async void ShareButton_Clicked(object sender, EventArgs e)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = $"https://www.ifpapinball.com/player.php?p={viewModel.PlayerId}",
                Title = "Share Player"
            });
        }

        private async void FavoriteButton_Clicked(object sender, EventArgs e)
        {

            if (await Settings.LocalDatabase.HasFavorite(viewModel.PlayerId))
            {
                await Settings.LocalDatabase.RemoveFavorite(viewModel.PlayerId);
                SetCorrectFavoriteIcon(false);
                await DisplayAlert("Favorite Removed", "This player has been removed from your favorites!", "OK");
            }
            else
            {
                await Settings.LocalDatabase.AddFavorite(viewModel.PlayerId);
                SetCorrectFavoriteIcon(true);                
                await DisplayAlert("Favorite Added", "This player has been added to your favorites!", "OK");
            }
        }

        private void SetCorrectFavoriteIcon(bool isFavorite = true)
        {
            if (isFavorite)
            {
                //if player is in the existing favorites list, fill the heart icon.
                ToolbarItems.SingleOrDefault(n => n.Text == "Favorite").IconImageSource = "favorite.png";
                if (Device.RuntimePlatform == Device.Android)
                {
                    ToolbarItems.SingleOrDefault(n => n.Text == "Favorite").IconImageSource = "favorite_white.png";
                }
                else
                {
                    ToolbarItems.SingleOrDefault(n => n.Text == "Favorite").IconImageSource = "favorite.png";
                }
            }
            else
            {                
                if (Device.RuntimePlatform == Device.Android)
                {
                    ToolbarItems.SingleOrDefault(n => n.Text == "Favorite").IconImageSource = "favorite_outline.png";
                }
                else
                {
                    ToolbarItems.SingleOrDefault(n => n.Text == "Favorite").IconImageSource = "favorite-outline.png";
                }
            }
        }

        private async void NACS_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PlayerChampionshipSeriesPage(new PlayerChampionshipSeriesViewModel(viewModel.PlayerId)));
        }
    }


}