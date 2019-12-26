using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Ifpa.ViewModels;
using PinballApi.Models.WPPR.v2.Directors;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DirectorsPage : ContentPage
    {
        DirectorsViewModel viewModel;

        public DirectorsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new DirectorsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var director = args.SelectedItem as Director;
            if (director == null)
                return;

            await this.Navigation.PushAsync(new PlayerDetailPage(new PlayerDetailViewModel(director.PlayerId)));

            // Manually deselect item.
            ((ListView)sender).SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            viewModel.LoadItemsCommand.Execute(null);
            base.OnAppearing();
        }
    }
}