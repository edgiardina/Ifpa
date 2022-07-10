namespace Ifpa.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
        }

        public string CurrentVersion => VersionTracking.CurrentVersion;
    }
}