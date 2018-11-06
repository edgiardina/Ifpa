using Xamarin.Forms;
using System;
using Xamarin.Forms.Xaml;
using System.Diagnostics;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfigureMyStatsPage : ContentPage
    {
        public ConfigureMyStatsPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            IfpaEntryField.TextColor = Color.Default;

            //Validate Player Id
            var isNumeric = int.TryParse(IfpaEntryField.Text, out int n);

            if (isNumeric)
            {
                try
                {
                    //Make sure this returns a real IFPA player
                    //var player = await viewModel.PinballRankingApi.GetPlayerRecord(n);

                    Application.Current.Properties["PlayerId"] = IfpaEntryField.Text;
                    await Navigation.PopModalAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            else
            {
                await DisplayAlert("IFPA Number invalid", "IFPA Number must be entered", "Cancel");
            }
        }
    }
}