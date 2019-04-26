using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;
using Ifpa.Models;
using System.Linq;
using System.Collections.Generic;

namespace Ifpa.ViewModels
{
    public class FavoritesViewModel : BaseViewModel
    {
        public ObservableCollection<PlayerWithWpprRank> Players { get; set; }
        public bool IsPopulated => Players.Count > 0 || dataNotLoaded;

        private bool dataNotLoaded = true;

        public FavoritesViewModel()
        {
            Title = "Favorites";
            Players = new ObservableCollection<PlayerWithWpprRank>();
        }

        private Command _loadItemsCommand;
        public Command LoadItemsCommand
        {
            get
            {
                return _loadItemsCommand ?? (_loadItemsCommand = new Command<string>(async (text) =>
                {
                    if (IsBusy)
                        return;

                    IsBusy = true;

                    dataNotLoaded = false;

                    try
                    {
                        Players.Clear();

                        var favorites = await Settings.LocalDatabase.GetFavorites();
                        var tempList = new List<PlayerWithWpprRank>();
                        
                        foreach (var item in favorites)
                        {
                            var playerRecord = await PinballRankingApi.GetPlayerRecord(item.PlayerID);

                            tempList.Add(new PlayerWithWpprRank(playerRecord.Player, playerRecord.PlayerStats.CurrentWpprRank));
                        }

                        foreach (var player in tempList.OrderBy(i => i.WpprRank))
                        {
                            Players.Add(player);
                        }

                        OnPropertyChanged("IsPopulated");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                    finally
                    {
                        IsBusy = false;
                    }
                }));
            }
        }

    }
}