using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.Toolkit.Parsers.Rss;
using Ifpa.Services;

namespace Ifpa.ViewModels
{
    public class NewsViewModel : BaseViewModel
    {
        public ObservableCollection<RssSchema> NewsItems { get; set; }
        public Command LoadItemsCommand { get; set; }

        private BlogPostService blogPostService { get; set; }
        
        public NewsViewModel()
        {
            blogPostService = new BlogPostService();

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
                var newsItems = await blogPostService.GetBlogPosts();

                foreach (var item in newsItems)
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


    }
}