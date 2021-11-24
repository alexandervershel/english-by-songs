using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Parsers
{
    public interface IWordsTranslationsParser
    {
        List<string> Translate(string word);
        Task<List<string>> TranslateAsync(string word);
    }
}