using Ifpa.Models;
using PinballApi.Extensions;
using PinballApi.Models.v2.WPPR;
using PinballApi.Models.WPPR.v2.Players;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Ifpa.ViewModels
{
    public class PlayerDetailViewModel : BaseViewModel
    {
        public Command LoadItemsCommand { get; set; }
        public Command PostPlayerLoadCommand { get; set; }

        public int PlayerId { get; set; }
        public int LastTournamentCount { get; set; }
        private Player playerRecord = new Player { PlayerStats = new PinballApi.Models.WPPR.v1.Players.PlayerStats { }, ChampionshipSeries = new System.Collections.Generic.List<ChampionshipSeries> { } };

        public Player PlayerRecord
        {
            get { return playerRecord; }
            set
            {
                playerRecord = value;
                OnPropertyChanged(null);
            }
        }
        public ObservableCollection<RankHistory> PlayerRankHistory { get; set; }

        public ObservableCollection<RatingHistory> PlayerRatingHistory { get; set; }

        public string Name => PlayerRecord.FirstName + " " + PlayerRecord.LastName;

        public string Rank => PlayerRecord.PlayerStats.CurrentWpprRank.OrdinalSuffix();

        public string Rating => PlayerRecord.PlayerStats.RatingsRank.HasValue ? PlayerRecord.PlayerStats.RatingsRank.Value.OrdinalSuffix() : "Not Ranked";

        public double? RatingValue => PlayerRecord.PlayerStats.RatingsValue;

        public string EffPercent => PlayerRecord.PlayerStats.EfficiencyRank.HasValue ? PlayerRecord.PlayerStats.EfficiencyRank.Value.OrdinalSuffix() : "Not Ranked";

        public double? EfficiencyValue => PlayerRecord.PlayerStats.EfficiencyValue;

        public double CurrentWpprValue => PlayerRecord.PlayerStats.CurrentWpprValue;

        public string LastMonthRank => PlayerRecord.PlayerStats.LastMonthRank.OrdinalSuffix();

        public string LastYearRank => PlayerRecord.PlayerStats.LastYearRank.OrdinalSuffix();

        public string HighestRank => PlayerRecord.PlayerStats.HighestRank.OrdinalSuffix();

        public DateTime? HighestRankDate => PlayerRecord.PlayerStats.HighestRankDate;

        public double TotalWpprs => PlayerRecord.PlayerStats.WpprPointsAllTime;

        public string BestFinish => PlayerRecord.PlayerStats.BestFinish.OrdinalSuffix();

        public int BestFinishCount => PlayerRecord.PlayerStats.BestFinishCount;

        public int AvgFinish => PlayerRecord.PlayerStats.AverageFinish;

        public int AvgFinishLastYear => PlayerRecord.PlayerStats.AverageFinishLastYear;

        public int TotalEvents => PlayerRecord.PlayerStats.TotalEventsAllTime;

        public int TotalActiveEvents => PlayerRecord.PlayerStats.TotalActiveEvents;
        
        public int EventsOutsideCountry => PlayerRecord.PlayerStats.TotalEventsAway;

        public string PlayerAvatar
        {
            get
            {
                if (PlayerRecord.ProfilePhoto != null)
                    return PlayerRecord.ProfilePhoto?.ToString();
                else
                    return Constants.PlayerProfileNoPicUrl;
            }
        }

        public bool HasNacsData => PlayerRecord.ChampionshipSeries != null;

        public string CountryFlag => $"https://www.countryflags.io/{PlayerRecord.CountryCode}/shiny/64.png";

        //Replace call at the end so that if a player is missing the 'state' we don't have an unsightly double space.
        public string Location => $"{PlayerRecord.City}{(!string.IsNullOrEmpty(PlayerRecord.City) && !string.IsNullOrEmpty(PlayerRecord.StateProvince) ? "," : string.Empty)} {PlayerRecord.StateProvince} {PlayerRecord.CountryName}".Trim().Replace("  ", " ");

        public bool IsRegistered => PlayerRecord.IfpaRegistered;
        
        public PlayerDetailViewModel(int? playerId = null)
        {
            if (playerId.HasValue)
                PlayerId = playerId.Value;

            PlayerRankHistory = new ObservableCollection<RankHistory>();
            PlayerRatingHistory = new ObservableCollection<RatingHistory>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            try
            {
                if (PlayerId > 0)
                {
                    IsBusy = true;
                    var playerData = await PinballRankingApiV2.GetPlayer(PlayerId);
                    var playerHistoryData = await PinballRankingApiV2.GetPlayerHistory(PlayerId);
                    LastTournamentCount = (await PinballRankingApiV2.GetPlayerResults(PlayerId)).ResultsCount;

                    if(playerHistoryData.RankHistory != null)
                        PlayerRankHistory = new ObservableCollection<RankHistory>(playerHistoryData.RankHistory);

                    if (playerHistoryData.RatingHistory != null)
                        PlayerRatingHistory = new ObservableCollection<RatingHistory>(playerHistoryData.RatingHistory);

                    PlayerRecord = playerData;
                    Title = PlayerRecord.Initials.ToUpper();

                    if (PostPlayerLoadCommand != null)
                    {
                        PostPlayerLoadCommand.Execute(null);
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
