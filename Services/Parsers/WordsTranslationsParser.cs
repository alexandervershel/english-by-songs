using HtmlAgilityPack;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Parsers
{
    public class WordsTranslationsParser : IWordsTranslationsParser
    {
        public List<string> Translate(string word)
        {
            List<string> translates = new List<string>();
            var path = "https://dictionary.cambridge.org/ru/словарь/англо-русский/" + word.Trim().Replace(" ", "%20");
            var web = new HtmlWeb();
            var doc = web.Load(path);
            var nodes = doc.DocumentNode.SelectNodes("//span[contains(@class, 'trans')]");
            if (nodes == null)
                return null;

            for (int i = 0; i < nodes.Count; i++)
            {
                //translates.AddRange(nodes[i].InnerText.Split(", "));
                translates.Add(nodes[i].InnerText.Trim());
            }

            return translates;

        }

        public async Task<List<string>> TranslateAsync(string word)
        {
            return await Task.Run(() => Translate(word));
        }
    }
}
