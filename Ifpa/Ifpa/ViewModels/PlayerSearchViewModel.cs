﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using PinballApi.Models.WPPR.Players;

namespace Ifpa.ViewModels
{
    public class PlayerSearchViewModel : BaseViewModel
    {
        public ObservableCollection<Search> Players { get; set; }
        
        public PlayerSearchViewModel()
        {
            Title = "Player Search";
            Players = new ObservableCollection<Search>();
        }

        private Command _searchCommand;
        public Command SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new Command<string>(async (text) =>
                {
                    if (IsBusy)
                        return;

                    IsBusy = true;

                    try
                    {
                        Players.Clear();
                        var items = await PinballRankingApi.SearchForPlayerByName(text);
                        foreach (var item in items.Search)
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
                }));
            }
        }

    }
}