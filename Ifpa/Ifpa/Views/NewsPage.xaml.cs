using Ifpa.ViewModels;
using Microsoft.Toolkit.Parsers.Rss;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.NewsItems.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        private async void ItemsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var newsItem = e.SelectedItem as RssSchema;
            if (newsItem == null)
                return;

            ItemsListView.SelectedItem = null;

            await Navigation.PushAsync(new NewsDetailPage(new NewsDetailViewModel(newsItem.FeedUrl)));
        }
    }
}