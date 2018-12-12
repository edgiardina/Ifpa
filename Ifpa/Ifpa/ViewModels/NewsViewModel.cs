using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.Toolkit.Parsers.Rss;
using System.Collections.Generic;
using System.Net.Http;
using Ifpa.Models;

namespace Ifpa.ViewModels
{
    public class NewsViewModel : BaseViewModel
    {
        public ObservableCollection<RssSchema> NewsItems { get; set; }
        public Command LoadItemsCommand { get; set; }
        
        public NewsViewModel()
        {
            Title = "News";
            NewsItems = new ObservableCollection<RssSchema>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                NewsItems.Clear();
                var tournamentResults = await Parse(Constants.IfpaRssFeedUrl);

                foreach (var item in tournamentResults)
                {                 
                    NewsItems.Add(item);                    
                }                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task<IEnumerable<RssSchema>> Parse(string url)
        {
            string feed = null;

            using (var client = new HttpClient())
            {
                feed = await client.GetStringAsync(url);
            }

            if (feed == null) return new List<RssSchema>();

            var parser = new RssParser();
            var rss = parser.Parse(feed);
            return rss;
        }
    }
}