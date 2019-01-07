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

        public RulesPage()
        {
            InitializeComponent();

            BindingContext = this.viewModel = new RulesViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.RulesContent == null)
                viewModel.LoadItemsCommand.Execute(null);
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
    }
}