using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpmlMaker
{
    public class OpmlFolder
    {
        public string TitleText { get; set; }

        public List<OpmlItem> Items { get; set; }

        public List<string> ToStringList()
        {
            var lines = new List<string>();
            lines.Add($"<outline title=\"{TitleText}\" text=\"{TitleText}\">");
            lines.AddRange(Items.Select(x => x.ToString()));
            lines.Add($"</outline>");
            return lines;
        }
    }

    public class OpmlItem
    {
        public string TitleText { get; set; }
        public string Type { get; set; } = "rss";
        public string XmlUrl { get; set; }

        public string ToString()
        {
            return $"<outline text=\"{TitleText}\" title=\"{TitleText}\" type=\"{Type}\" xmlUrl=\"{XmlUrl}\"/>";
        }
    }
}

/*
 <outline title="News" text="News">
    <outline text="Big News Finland" title="Big News Finland" type="rss" xmlUrl="http://www.bignewsnetwork.com/?rss=37e8860164ce009a"/>
    <outline text="Euronews" title="Euronews" type="rss" xmlUrl="http://feeds.feedburner.com/euronews/en/news/"/>
    <outline text="Reuters Top News" title="Reuters Top News" type="rss" xmlUrl="http://feeds.reuters.com/reuters/topNews"/>
    <outline text="Yahoo Europe" title="Yahoo Europe" type="rss" xmlUrl="http://rss.news.yahoo.com/rss/europe"/>
</outline>
 */