using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using PinballApi.Models.WPPR.v1.Pvp;
using System.Linq;

namespace Ifpa.ViewModels
{
    public class PlayerVersusPlayerDetailViewModel : BaseViewModel
    {
        public ObservableCollection<Pvp> PlayerVersusPlayer { get; set; }

        public string PlayerOneInitials { get; set; }

        public string PlayerTwoInitials { get; set; }
        public Command LoadItemsCommand { get; set; }

        private int playerOneId;
        private int playerTwoId;

        public PlayerVersusPlayerDetailViewModel(int playerOneId, int playerTwoId)
        {
            this.playerOneId = playerOneId;
            this.playerTwoId = playerTwoId;
            PlayerVersusPlayer = new ObservableCollection<Pvp>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                PlayerVersusPlayer.Clear();
                var pvpResults = await PinballRankingApi.GetPvp(playerOneId, playerTwoId);

                foreach (var item in pvpResults.Pvp.OrderByDescending(n => n.EventDate))
                {                 
                    PlayerVersusPlayer.Add(item);
                }

                Title = $"{pvpResults.P1FirstName} {pvpResults.P1LastName} vs {pvpResults.P2FirstName} {pvpResults.P2LastName}";

                PlayerOneInitials = pvpResults.P1FirstName.FirstOrDefault().ToString() + pvpResults.P1LastName.FirstOrDefault().ToString();
                PlayerTwoInitials = pvpResults.P2FirstName.FirstOrDefault().ToString() + pvpResults.P2LastName.FirstOrDefault().ToString();
                OnPropertyChanged("PlayerOneInitials");
                OnPropertyChanged("PlayerTwoInitials");
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