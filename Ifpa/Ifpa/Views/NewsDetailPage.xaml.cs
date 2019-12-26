using Ifpa.ViewModels;
using System;
using System.Collections;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsDetailPage : ContentPage
    {
        NewsDetailViewModel viewModel;

        public NewsDetailPage( NewsDetailViewModel model)
        {
            InitializeComponent();

            BindingContext = this.viewModel = model;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.NewsItem == null)
                viewModel.LoadItemsCommand.Execute(null);
        }

        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            if (viewModel.CommentCounts > 0)
            {
                ItemsListView.IsVisible = !ItemsListView.IsVisible;
                ItemsListView.ScrollTo(((IList)ItemsListView.ItemsSource)[0], ScrollToPosition.Start, false);
            }
        }

        private void WebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            //open all links in a new browser window
            if (e.Url.StartsWith("http"))
            {
                try
                {
                    var uri = new Uri(e.Url);               
                    Launcher.OpenAsync(uri);
                }
                catch (Exception)
                {
                }

                e.Cancel = true;
            }
        }
    }
}