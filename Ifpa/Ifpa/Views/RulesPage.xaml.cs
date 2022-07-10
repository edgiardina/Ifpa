using Ifpa.ViewModels;
using System;
using System.Collections;
using Microsoft.Maui;


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
            viewModel.LoadItemsCommand.Execute(null);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}