﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;
using Ifpa.Models;
using System.Linq;
using System.Collections.Generic;
using PinballApi.Models.WPPR.v2.Directors;

namespace Ifpa.ViewModels
{
    public class DirectorsViewModel : BaseViewModel
    {
        public ObservableCollection<Director> NacsDirectors { get; set; }
        public ObservableCollection<Director> CountryDirectors { get; set; }

        public DirectorsViewModel()
        {
            Title = "Directors";
            NacsDirectors = new ObservableCollection<Director>();
            CountryDirectors = new ObservableCollection<Director>();
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
                    
                    try
                    {
                        NacsDirectors.Clear();
                        CountryDirectors.Clear();
                      
                        var nacsDirectors = await PinballRankingApiV2.GetNacsDirectors();
                        var countryDirectors = await PinballRankingApiV2.GetCountryDirectors();

                        foreach (var director in nacsDirectors)
                        {
                            NacsDirectors.Add(director);
                        }

                        foreach (var director in countryDirectors)
                        {
                            CountryDirectors.Add(director);
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