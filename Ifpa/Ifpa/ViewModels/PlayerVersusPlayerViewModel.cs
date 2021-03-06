﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using PinballApi.Models.WPPR.v1.Players;
using System.Linq;
using Ifpa.Models;

namespace Ifpa.ViewModels
{
    public class PlayerVersusPlayerViewModel : BaseViewModel
    {
        public ObservableCollection<Grouping<char, PlayerVersusRecord>> Results { get; set; }
        public Command LoadItemsCommand { get; set; }

        public int PlayerId { get; set; }

        public bool HasNoPvpData { get; set; }

        public PlayerVersusPlayerViewModel(int playerId)
        {
            Title = "PVP";
            this.PlayerId = playerId;
            Results = new ObservableCollection<Grouping<char, PlayerVersusRecord>>();
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
                var pvpResults = await PinballRankingApi.GetPlayerComparisons(PlayerId);

                if (pvpResults.Pvp != null)
                {
                    var lastNames = pvpResults.Pvp
                                            .OrderBy(n => n.LastName).Select(n => n.LastName).ToList();
                    var groupedResults = pvpResults.Pvp
                                            .OrderBy(n => n.LastName)
                                            .ThenBy(n => n.FirstName)
                                            .GroupBy(c => char.ToUpper(c.LastName.FirstOrDefault()))
                                            .Select(g => new Grouping<char, PlayerVersusRecord>(g.Key, g));

                    foreach (var item in groupedResults)
                    {
                        Results.Add(item);
                    }
                }
                else
                {
                    HasNoPvpData = true;

                    OnPropertyChanged(nameof(HasNoPvpData));
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