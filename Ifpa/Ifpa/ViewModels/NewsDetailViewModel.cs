﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using System.ServiceModel.Syndication;
using Ifpa.Services;
using System.Xml;

namespace Ifpa.ViewModels
{
    public class NewsDetailViewModel : BaseViewModel
    {
        public SyndicationItem NewsItem { get; set; }
        public ObservableCollection<SyndicationItem> Comments { get; set; }

        public int CommentCounts => Comments.Count;
        public Command LoadItemsCommand { get; set; }

        public Uri NewsItemUrl { get; set; }
        public HtmlWebViewSource NewsItemContent { get; set; }

        private BlogPostService blogPostService { get; set; }

        public NewsDetailViewModel()
        {
            Title = "News";
            blogPostService = new BlogPostService();

            Comments = new ObservableCollection<SyndicationItem>();
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
                var newsItemDetails = await blogPostService.GetBlogPosts();

                NewsItem = newsItemDetails.Single(n => n.Links.Any(m => m.Uri == NewsItemUrl));
                OnPropertyChanged(nameof(NewsItem));
                Title = NewsItem.Title.Text;
                NewsItemContent = new HtmlWebViewSource();
                //TODO: make this HTML and CSS in discrete files 
                var articleContent = NewsItem.ElementExtensions
                      .FirstOrDefault(ext => ext.OuterName == "encoded")
                      .GetObject<XmlElement>().InnerText;

                NewsItemContent.Html = "<html><head><style>img {display:block; clear:both;  margin: auto; margin-botton:10px !important;}</style></head><body style='font-family:sans-serif;'>" + articleContent + "</body></html>";
                OnPropertyChanged(nameof(NewsItemContent));

                var comments = await blogPostService.GetCommentsForBlogPost(NewsItem.Id);

                foreach (var item in comments)
                {
                    Comments.Add(item);                    
                }
                OnPropertyChanged(nameof(CommentCounts));
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