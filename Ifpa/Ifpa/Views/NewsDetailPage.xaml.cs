using Ifpa.ViewModels;
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

    }
}