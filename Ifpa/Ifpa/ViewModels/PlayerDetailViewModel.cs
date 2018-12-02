using PinballApi.Extensions;
using PinballApi.Models.WPPR.Players;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Ifpa.ViewModels
{
    public class PlayerDetailViewModel : BaseViewModel
    {
        public Command LoadItemsCommand { get; set; }

        public int PlayerId { get; set; }
        public int LastTournamentCount { get; set; }
        private PlayerRecord playerRecord = new PlayerRecord { Player = new Player { }, PlayerStats = new PlayerStats { } };

        public PlayerRecord PlayerRecord
        {
            get { return playerRecord; }
            set
            {
                playerRecord = value;
                OnPropertyChanged(null);
            }
        }

        public string Name => PlayerRecord.Player.FirstName + " " + PlayerRecord.Player.LastName;

        public string Rank => PlayerRecord.PlayerStats.CurrentWpprRank.OrdinalSuffix();

        public string Rating => PlayerRecord.PlayerStats.RatingsRank.OrdinalSuffix();

        public double RatingValue => PlayerRecord.PlayerStats.RatingsValue;

        public string EffPercent => PlayerRecord.PlayerStats.EfficiencyRank.HasValue ? PlayerRecord.PlayerStats.EfficiencyRank.Value.OrdinalSuffix() : "Not Ranked";

        public double? EfficiencyValue => PlayerRecord.PlayerStats.EfficiencyValue;

        public double TotalWpprs => PlayerRecord.PlayerStats.CurrentWpprValue;

        public string PlayerAvatar
        {
            get
            {
                var actualUrl = $"https://www.ifpapinball.com/images/profiles/players/{PlayerId}.jpg";
                var httpClient = new HttpClient();                

                var uri = new Uri(actualUrl);
                using (var response = httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead).Result)
                {
                    if (!response.IsSuccessStatusCode)
                        return "https://www.ifpapinball.com/images/noplayerpic.png";
                }
                return actualUrl;
            }
        }

        public string CountryFlag => $"https://www.countryflags.io/{PlayerRecord.Player.CountryCode}/shiny/64.png";

        public string Location => $"{PlayerRecord.Player.City} {PlayerRecord.Player.State} {PlayerRecord.Player.CountryName}";

        public bool IsRegistered => PlayerRecord.Player.IfpaRegistered == "Y";

        public PlayerDetailViewModel(int playerId)
        {
            this.PlayerId = playerId;
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            try
            {         

                IsBusy = true;
                var playerData = await PinballRankingApi.GetPlayerRecord(PlayerId);
                LastTournamentCount = (await PinballRankingApi.GetPlayerResults(PlayerId)).ResultsCount;

                PlayerRecord = playerData;
                Title = PlayerRecord.Player.Initials;
            }
            catch(Exception ex) { }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
