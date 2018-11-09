using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Ifpa.ViewModels;
using System;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarDetailPage : ContentPage
    {
        CalendarDetailViewModel viewModel;
        
        public CalendarDetailPage(CalendarDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;            
        }


        protected async override void OnAppearing()
        {
            base.OnAppearing();          

            viewModel.LoadItemsCommand.Execute(null);
        }
    }
}