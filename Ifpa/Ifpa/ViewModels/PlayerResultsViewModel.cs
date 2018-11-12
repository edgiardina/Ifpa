﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using PinballApi.Models.WPPR.Players;

namespace Ifpa.ViewModels
{
    public class PlayerResultsViewModel : BaseViewModel
    {
        public ObservableCollection<Result> Results { get; set; }
        public Command LoadItemsCommand { get; set; }

        private int playerId;

        public PlayerResultsViewModel(int playerId)
        {
            Title = "Player Results";
            this.playerId = playerId;
            Results = new ObservableCollection<Result>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Results.Clear();
                var playerResults = await PinballRankingApi.GetPlayerResults(playerId);

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