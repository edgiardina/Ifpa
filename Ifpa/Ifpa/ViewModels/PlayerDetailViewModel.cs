using PinballApi.Extensions;
using PinballApi.Models.WPPR.Players;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Ifpa.ViewModels
{
    public class PlayerDetailViewModel : BaseViewModel
    {
        public Command LoadItemsCommand { get; set; }

        public int PlayerId { get; set; }
        private PlayerRecord playerRecord = new PlayerRecord { Player = new Player { }, PlayerStats = new PlayerStats { } };

        public PlayerRecord PlayerRecord
        {
            get { return playerRecord; }
            set
            {
                playerRecord = value;
                OnPropertyChanged("Name");
                OnPropertyChanged("Rank");
                OnPropertyChanged("TotalWpprs");
                OnPropertyChanged("PlayerAvatar");
            }
        }

        public string Name => PlayerRecord.Player.FirstName + " " + PlayerRecord.Player.LastName;

        public string Rank => PlayerRecord.PlayerStats.CurrentWpprRank.OrdinalSuffix(); 

        public double TotalWpprs => PlayerRecord.PlayerStats.CurrentWpprValue;

        public string PlayerAvatar => $"https://www.ifpapinball.com/images/profiles/players/{PlayerId}.jpg";

        public PlayerDetailViewModel(int playerId)
        {
            this.PlayerId = playerId;
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            try
            {
                var playerData = await PinballRankingApi.GetPlayerRecord(PlayerId);

                PlayerRecord = playerData;
                Title = PlayerRecord.Player.Initials;
            }
            catch(Exception ex) { }
        }

    }
}
