using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using PinballApi.Models.WPPR.Tournaments;
using PinballApi.Models.WPPR.Players;
using System.Linq;

namespace Ifpa.ViewModels
{
    public class PlayerVersusPlayerViewModel : BaseViewModel
    {
        public ObservableCollection<PlayerVersusRecord> Results { get; set; }
        public Command LoadItemsCommand { get; set; }

        private int playerId;

        public PlayerVersusPlayerViewModel(int playerId)
        {
            Title = "PVP";
            this.playerId = playerId;
            Results = new ObservableCollection<PlayerVersusRecord>();
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
                var pvpResults = await PinballRankingApi.GetPlayerComparisons(playerId);

                foreach (var item in pvpResults.Pvp.OrderBy(n => n.LastName).ThenBy(n => n.FirstName))
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