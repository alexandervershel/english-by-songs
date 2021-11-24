using System.Threading.Tasks;

namespace Services.Parsers
{
    public interface ISongLyricsParser
    {
        string Parse(string artist, string songName);
        Task<string> ParseAsync(string artist, string songName);
    }
}