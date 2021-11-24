using HtmlAgilityPack;
using System.Threading.Tasks;

namespace Services.Parsers
{
    public class SongLyricsParser : ISongLyricsParser
    {
        public string MainUrl { get; } = "https://www.azlyrics.com/lyrics";
        public string Parse(string artist, string songName)
        {
            string fullUrl = $"{MainUrl}/{artist.Trim().ToLower().Replace(" ", string.Empty)}/{songName.Trim().ToLower().Replace(" ", string.Empty)}.html";
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

        public Task<string> ParseAsync(string artist, string songName)
        {
            return Task.Run(() => Parse(artist, songName));
        }
    }
}
