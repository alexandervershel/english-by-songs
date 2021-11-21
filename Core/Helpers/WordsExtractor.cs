using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Core.Helpers
{
    public static class WordsExtractor
    {
        public static IEnumerable<string> Extract(string text)
        {
            text = Regex.Replace(text, "\r\n|\r|\n[,.:;!?]", " ");
            text = Regex.Replace(text, "[^a-zA-Z ']+", " ");
            text = Regex.Replace(text, @"\s+", " ");

            List<string> words = new List<string>(text.Split(" "));
            words = words.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            words = words.ConvertAll(x => x.ToLower());

            return words;
        }
    }
}
