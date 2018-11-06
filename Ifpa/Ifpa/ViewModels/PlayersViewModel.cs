﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using PinballApi.Models.WPPR.Rankings;

namespace Ifpa.ViewModels
{
    public class PlayersViewModel : BaseViewModel
    {
        public ObservableCollection<Ranking> Players { get; set; }
        public Command LoadItemsCommand { get; set; }

        public PlayersViewModel()
        {
            Title = "Top 50";
            Players = new ObservableCollection<Ranking>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Players.Clear();
                var items = await PinballRankingApi.GetRankings();
                foreach (var item in items.Rankings)
                {
                    Players.Add(item);
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