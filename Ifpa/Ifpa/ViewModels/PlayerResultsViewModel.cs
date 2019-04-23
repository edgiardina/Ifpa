using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using PinballApi.Models.WPPR.v1.Players;
using System.Linq;

namespace Ifpa.ViewModels
{
    public enum PlayerResultState
    {
        Active,
        NonActive,
        Inactive
    }

    public class PlayerResultsViewModel : BaseViewModel
    {
        public ObservableCollection<Result> Results { get; set; }
        public Command LoadItemsCommand { get; set; }

        public PlayerResultState State { get; set; }

        private int playerId;

        public PlayerResultsViewModel(int playerId)
        {
            Title = "Results";
            this.playerId = playerId;
            State = PlayerResultState.Active;
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

                foreach (var item in playerResults.Results.Where(n => n.WpprState == State.ToString()))
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