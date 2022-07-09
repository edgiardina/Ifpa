using Microsoft.Maui;
using Microsoft.Maui.Xaml;
using Ifpa.ViewModels;
using PinballApi.Models.WPPR.v2.Players;
using PinballApi.Models.WPPR.v2.Rankings;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomRankingsPage : ContentPage
    {
        CustomRankingsViewModel viewModel;

        public CustomRankingsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new CustomRankingsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var view = args.SelectedItem as CustomRankingView;
            if (view == null)
                return;

            await Navigation.PushAsync(new CustomRankingsDetailPage(view.ViewId));

            // Manually deselect item.
            RankingsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            if(viewModel.CustomRankings.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);

            base.OnAppearing();
        }
    }
}