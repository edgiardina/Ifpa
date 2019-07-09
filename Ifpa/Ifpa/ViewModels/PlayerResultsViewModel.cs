using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using PinballApi.Models.WPPR.v2.Players;
using PinballApi.Models.WPPR.v2;
using System.Collections.Generic;

namespace Ifpa.ViewModels
{
    public class PlayerResultsViewModel : BaseViewModel
    {
        public ObservableCollection<PlayerResult> ActiveResults { get; set; }
        public ObservableCollection<PlayerResult> UnusedResults { get; set; }
        public ObservableCollection<PlayerResult> PastResults { get; set; }

        public Command LoadItemsCommand { get; set; }

        public ResultType State { get; set; }
        public RankingType RankingType { get; set; }

        public RankingType[] RankingTypeOptions => new[] { RankingType.Main, RankingType.Women, RankingType.Youth };
     
        public bool ShowRankingTypeChoice { get; set; }

        private int playerId;

        public PlayerResultsViewModel(int playerId)
        {
            Title = "Results";
            this.playerId = playerId;
            State = ResultType.Active;
            RankingType = RankingType.Main;
            ShowRankingTypeChoice = false;
            ActiveResults = new ObservableCollection<PlayerResult>();
            UnusedResults = new ObservableCollection<PlayerResult>();
            PastResults = new ObservableCollection<PlayerResult>();

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var player = await PinballRankingApiV2.GetPlayer(playerId);

                //TODO: it would be better to get a list of all categories a player is eligible for
                if (player.Gender == Gender.Female || (player.Age.HasValue && player.Age.Value < 18))
                {
                    ShowRankingTypeChoice = true;
                    OnPropertyChanged(nameof(ShowRankingTypeChoice));
                }

                ActiveResults.Clear();
                UnusedResults.Clear();
                PastResults.Clear();

                var activeResults = await PinballRankingApiV2.GetPlayerResults(playerId, RankingType, ResultType.Active);
                if (activeResults.ResultsCount > 0)
                {
                    foreach (var item in activeResults.Results)
                    {
                        ActiveResults.Add(item);
                    }
                }
                
                var unusedResults = await PinballRankingApiV2.GetPlayerResults(playerId, RankingType, ResultType.NonActive);
                if (unusedResults.ResultsCount > 0)
                {
                    foreach (var item in unusedResults.Results)
                    {
                        UnusedResults.Add(item);
                    }
                }
                
                var pastResults = await PinballRankingApiV2.GetPlayerResults(playerId, RankingType, ResultType.Inactive);
                if (pastResults.ResultsCount > 0)
                {
                    foreach (var item in pastResults.Results)
                    {
                        PastResults.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}