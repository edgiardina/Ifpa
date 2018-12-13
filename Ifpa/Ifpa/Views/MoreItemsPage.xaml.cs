using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MoreItemsPage : ContentPage
    {
        public MoreItemsPage()
        {
            InitializeComponent();
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (listView.SelectedItem != null)
            {
                await Navigation.PushAsync((Page)Activator.CreateInstance(((MoreItemsMenuItem)e.SelectedItem).TargetType));
                listView.SelectedItem = null;
            }
        }
    }
}