using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace EnglishBySongs
{
    class SongLyricsParser
    {
        public SongLyricsParser(string performer, string songName)
        {
            _performer = performer;
            _songName = songName;
        }

        private string _performer;

        private string _songName;

        public string MainUrl { get; } = "https://www.azlyrics.com/lyrics";

        public string Parse()
        {
            string fullUrl = $"{MainUrl}/{_performer.Trim().ToLower().Replace(" ", string.Empty)}/{_songName.Trim().ToLower().Replace(" ", string.Empty)}.html";
            var web = new HtmlWeb();
            var doc = web.Load(fullUrl);
            HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[not(@id) and not(@class)]");
            if (node == null)
                return null;

            string result = node.InnerText.Replace("&quot;", "");

            while (result.Contains('['))
            {
                result = result.Remove(result.IndexOf('['), result.IndexOf(']') - result.IndexOf('[') + 1);
            }
            return result.Trim();
        }

        public Task<string> ParseAsync()
        {
            return Task.Run(() => Parse());
        }
    }
}
