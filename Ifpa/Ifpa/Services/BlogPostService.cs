using Ifpa.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;

namespace Ifpa.Services
{
    public class BlogPostService
    {
        public async Task<IEnumerable<SyndicationItem>> GetBlogPosts()
        {
            return await Parse(Constants.IfpaRssFeedUrl);
        }

        public async Task<IEnumerable<SyndicationItem>> GetCommentsForBlogPost(string blogPostId)
        {
            var blogPosts = await Parse(Constants.IfpaRssFeedUrl);
            var post = blogPosts.Single(n => n.Id == blogPostId);
            var link = post.Links.FirstOrDefault().Uri.ToString();

            return await Parse(link + "/feed");
        }

        public int ParseGuidFromInternalId(string internalId)
        {
            //parse url and return integer p value from the following url style
            //https://www.ifpapinball.com/?p=12345
            return int.Parse(internalId.Split('=')[1]);
        }

        private async Task<IEnumerable<SyndicationItem>> Parse(string url)
        {
            Stream stream = null;

            using (var client = new HttpClient())
            {
                stream = await client.GetStreamAsync(url);
            }
            
            if (stream == null) return new List<SyndicationItem>();

            XmlReader reader = XmlReader.Create(stream);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
     
            return feed.Items;
        }
    }
}
