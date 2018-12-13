using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.Toolkit.Parsers.Rss;
using System.Collections.Generic;
using System.Net.Http;
using Ifpa.Models;
using System.Linq;

namespace Ifpa.ViewModels
{
    public class NewsDetailViewModel : BaseViewModel
    {
        public RssSchema NewsItem { get; set; }
        public ObservableCollection<RssSchema> Comments { get; set; }
        public Command LoadItemsCommand { get; set; }

        public string NewsItemUrl { get; set; }
        public HtmlWebViewSource NewsItemContent { get; set; }
        
        public NewsDetailViewModel(string url)
        {
            Title = "News";
            NewsItemUrl = url;
            Comments = new ObservableCollection<RssSchema>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Comments.Clear();
                var newsItemDetails = await Parse(Constants.IfpaRssFeedUrl);

                NewsItem = newsItemDetails.Single(n => n.FeedUrl == NewsItemUrl);
                OnPropertyChanged(nameof(NewsItem));
                Title = NewsItem.Title;
                NewsItemContent = new HtmlWebViewSource();
                NewsItemContent.Html = "<html><head><style>img {display:block; clear:both;  margin: auto;}</style></head><body>" + NewsItem.Content + "</body></html>";
                OnPropertyChanged(nameof(NewsItemContent));

                var comments = await Parse(NewsItem.FeedUrl + "feed/");

                foreach (var item in comments)
                {                 
                    Comments.Add(item);                    
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