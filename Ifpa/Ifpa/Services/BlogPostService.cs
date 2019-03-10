using Ifpa.Models;
using Microsoft.Toolkit.Parsers.Rss;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ifpa.Services
{
    public class BlogPostService
    {
        public async Task<IEnumerable<RssSchema>> GetBlogPosts()
        {
            return await Parse(Constants.IfpaRssFeedUrl);
        }

        public int ParseGuidFromInternalId(string internalId)
        {
            //parse url and return integer p value from the following url style
            //https://www.ifpapinball.com/?p=12345
            return int.Parse(internalId.Split('=')[1]);
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
