using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace A2.Web.Models
{
    public class NewsModels
    {
        public static async Task<List<NewsItem>> GetNews(string RssURI)
        {
            var x = XElement.Load(RssURI);
            DateTime _out = default(DateTime);
            return x.Element("channel").Elements("item")
                .Select(y => new NewsItem()
                {
                    Content = (y.Element("description") ?? new XElement("description")).Value,
                    PostDate = DateTime.TryParse((y.Element("pubDate") ?? new XElement("pubDate", default(DateTime))).Value, out _out) ?
                                    DateTime.Parse((y.Element("pubDate") ?? new XElement("pubDate", default(DateTime))).Value) :
                                    default(DateTime),
                    SourceURL = (y.Element("link") ?? new XElement("link")).Value,
                    Title = (y.Element("title") ?? new XElement("title")).Value
                }).ToList();
        }

    }
}