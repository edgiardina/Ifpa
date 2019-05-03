﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using PinballApi.Models.WPPR.v2.Players;
using PinballApi.Models.WPPR.v2;

namespace Ifpa.ViewModels
{
    public class PlayerResultsViewModel : BaseViewModel
    {
        public ObservableCollection<PlayerResult> Results { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ResultType State { get; set; }
        public RankingType RankingType { get; set; }

        public bool ShowRankingTypeChoice { get; set; }

        private int playerId;

        public PlayerResultsViewModel(int playerId)
        {
            Title = "Results";
            this.playerId = playerId;
            State = ResultType.Active;
            RankingType = RankingType.Main;
            ShowRankingTypeChoice = false;
            Results = new ObservableCollection<PlayerResult>();
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

                Results.Clear();
                var playerResults = await PinballRankingApiV2.GetPlayerResults(playerId, RankingType, State);

                foreach (var item in playerResults.Results)
                {                 
                    Results.Add(item);
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