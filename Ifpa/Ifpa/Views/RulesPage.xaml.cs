using Ifpa.ViewModels;
using System;
using System.Collections;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RulesPage : ContentPage
    {
        RulesViewModel viewModel;

        string unmarkExistingMarks = "instance.unmark();";

        public RulesPage()
        {
            InitializeComponent();

            BindingContext = this.viewModel = new RulesViewModel();
            viewModel.LoadItemsCommand.Execute(null);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void WebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            //open all links in a new browser window
            if (e.Url.StartsWith("http"))
            {
                try
                {
                    var uri = new Uri(e.Url);
                    Device.OpenUri(uri);
                }
                catch (Exception)
                {
                }

                e.Cancel = true;
            }
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            SearchBar.IsVisible = !SearchBar.IsVisible;

            if (!SearchBar.IsVisible)
            {
                await RulesWebView.EvaluateJavaScriptAsync(unmarkExistingMarks);
            }
        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {            
            var markSearchTerm = $@"instance.mark('{e.NewTextValue}');";
            var scrollToMark = $@"jumpTo('{e.NewTextValue}');";

            try
            {
                await RulesWebView.EvaluateJavaScriptAsync(unmarkExistingMarks);
                await RulesWebView.EvaluateJavaScriptAsync(markSearchTerm);
                await RulesWebView.EvaluateJavaScriptAsync(scrollToMark);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            var searchTerm = ((SearchBar)sender).Text;
            var scrollToMark = $@"jumpTo('{searchTerm}');";

            try
            {
                await RulesWebView.EvaluateJavaScriptAsync(scrollToMark);
                SearchBar.Focus();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}