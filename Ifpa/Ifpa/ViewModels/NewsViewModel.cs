using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Maui;
using Ifpa.Services;
using System.ServiceModel.Syndication;
using System.Web;

namespace Ifpa.ViewModels
{
    public class NewsViewModel : BaseViewModel
    {
        public ObservableCollection<SyndicationItem> NewsItems { get; set; }
        public Command LoadItemsCommand { get; set; }

        private BlogPostService blogPostService { get; set; }

        public NewsViewModel()
        {
            blogPostService = new BlogPostService();

            Title = "News";
            NewsItems = new ObservableCollection<SyndicationItem>();
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
                var newsItems = await blogPostService.GetBlogPosts();

                foreach (var item in newsItems)
                {
                    item.Summary = new TextSyndicationContent(HttpUtility.HtmlDecode(item.Summary.Text));
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


    }
}