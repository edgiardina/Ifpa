using Ifpa.ViewModels;
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

        private void ItemsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}