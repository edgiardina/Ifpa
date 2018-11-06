using PinballApi.Extensions;
using PinballApi.Models.WPPR.Players;

namespace Ifpa.ViewModels
{
    public class PlayerDetailViewModel : BaseViewModel
    {
        public PlayerRecord PlayerRecord { get; set; }

        public string Name => PlayerRecord.Player.FirstName + " " + PlayerRecord.Player.LastName;
        public Player Player => PlayerRecord.Player;
        public string Rank => PlayerRecord.PlayerStats.CurrentWpprRank.OrdinalSuffix();

        public double TotalWpprs => PlayerRecord.PlayerStats.CurrentWpprValue;

        public string PlayerAvatar => $"https://www.ifpapinball.com/images/profiles/players/{Player.PlayerId}.jpg"; 

        public PlayerDetailViewModel(PlayerRecord player = null)
        {
            PlayerRecord = player;
            Title = player.Player.Initials;            
        }
    }
}
