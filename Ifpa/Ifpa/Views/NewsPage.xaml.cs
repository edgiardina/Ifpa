using Ifpa.ViewModels;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using Microsoft.Maui;


namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsPage : ContentPage
    {
        NewsViewModel viewModel;

        public NewsPage()
        {
            InitializeComponent();

            BindingContext = this.viewModel = new NewsViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.NewsItems.Count == 0)
            {                
                await Task.Run(() => viewModel.LoadItemsCommand.Execute(null));
            }
        }

        private async void ItemsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var newsItem = e.SelectedItem as SyndicationItem;
            if (newsItem == null)
                return;

            ItemsListView.SelectedItem = null;

            await Shell.Current.GoToAsync($"news-detail?newsUri={System.Uri.EscapeDataString(newsItem.Links.FirstOrDefault().Uri.ToString())}");    
        }
    }
}