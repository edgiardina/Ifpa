using Ifpa.Models;
using Ifpa.Views;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ifpa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("player-search", typeof(PlayerSearchPage));
            Routing.RegisterRoute("player-results", typeof(PlayerResultsPage));
            Routing.RegisterRoute("player-details", typeof(PlayerDetailPage));
            Routing.RegisterRoute("pvp", typeof(PlayerVersusPlayerPage));
            Routing.RegisterRoute("pvp-detail", typeof(PlayerVersusPlayerDetailPage));
            Routing.RegisterRoute("activity-feed", typeof(ActivityFeedPage));

            Routing.RegisterRoute("champ-series", typeof(ChampionshipSeriesPage));
            Routing.RegisterRoute("champ-series-detail", typeof(ChampionshipSeriesDetailPage));
            Routing.RegisterRoute("champ-series-player", typeof(ChampionshipSeriesPlayerCardPage));

            Routing.RegisterRoute("tournament-results", typeof(TournamentResultsPage));

            Routing.RegisterRoute("calendar-detail", typeof(CalendarDetailPage));
        }

        protected override async void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);

            //var i = this.Children.IndexOf(this.CurrentPage);

            //Settings.CurrentTabIndex = i;

            //await Application.Current.SavePropertiesAsync();

            ShellNavigatingDeferral token = args.GetDeferral();

            //If a user hasn't set up my stats, redirect to player search

            try
            {
                if (!Settings.HasConfiguredMyStats                                                                            
                    && args.Target.Location.ToString().Contains("my-stats")
                )
                {
                    await DisplayAlert("Configure your Stats", "Looks like you haven't configured your 'My Stats' page. Use the Player Search under 'Rankings' to find your Player, and press the Star to configure your Stats", "OK");
                    args.Cancel();
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            token?.Complete();
        }
    }
}