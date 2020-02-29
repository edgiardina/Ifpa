using Xamarin.Forms;

namespace Ifpa.Models
{
    /// <summary>
    /// HomeTabbedPage allows reselection of bottom tab buttons for android
    /// </summary>
    public class HomeTabbedPage : TabbedPage
    {
        public async void NotifyTabReselected()
        {
            await CurrentPage.Navigation.PopToRootAsync();
        }
    }    
}
