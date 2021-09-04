using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using OpmlMaker;
using System.Net;

namespace YoutubeRegexParser
{
    public class YoutubeRegexParser
    {
        public static void Test()
        {
            var regex = new Regex("role=\"tablist\" title=\"[^\"]*\" href=\"[^\"]*\"", RegexOptions.IgnoreCase);
            var text = File.ReadAllText(@"C:\Users\Mikhail\Downloads\111\(3857) YouTube.htm");
            var result = regex.Matches(text);
            var a = result; //role="tablist" title="Filinov's Place" href="https://www.youtube.com/c/filinobzor01"
            var b = result.Skip(10);
            var tuples = b.Select(x => new Tuple<string, string>(
                x.Value.Substring(22).Split('\"')[0], 
                x.Value.Split('\"').First(s => s.Contains("youtube.com"))));

            var dee = tuples.Select(x => new Tuple<string, string>(x.Item1, ExtractChannelIdFromChannelUrl(x.Item2))).ToList();

            var feeds = dee.Select(x => new Tuple<string, string>(x.Item1, "https://www.youtube.com/feeds/videos.xml?channel_id=" + x.Item2));

            var of1 = new OpmlFolder();
            of1.TitleText = "Youtube-1-Test";
            of1.Items = feeds.Select(x => new OpmlItem() {TitleText = x.Item1, XmlUrl = x.Item2}).ToList();

            var opmlLines = OpmlMaker.OpmlMaker.MakeOpmlStringList(new List<OpmlFolder>(){of1});

            File.WriteAllLines("C:\\Users\\Mikhail\\Downloads\\MY_OPML3.xml", opmlLines);

            Console.WriteLine("File Written!!!");
        }

        public static string ExtractChannelIdFromChannelUrl(string channelUrl)
        {
            var pageString = "";
            using (WebClient web1 = new WebClient())
            {
                Console.WriteLine("Requesting page: " + channelUrl);
                pageString = web1.DownloadString(channelUrl);
            }
            var regexChannelPageId = new Regex("<meta itemprop=\"channelId\" content=\"[^\"]*\"", RegexOptions.IgnoreCase); //<meta itemprop="channelId" content="UCkfUjgaPa-V0WE-6zLYUSuw">
            try
            {
                return regexChannelPageId.Matches(pageString).First().Value.Substring(35).Trim('\"');
            }
            catch
            {
                return "NULL";
            }
        }
    }
}
