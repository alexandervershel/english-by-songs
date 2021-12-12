using Entities;
using System.Collections.Generic;

namespace EnglishBySongs.ViewModels.ItemViewModels
{
    public class SongItemViewModel : BaseItemViewModel<Song>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Lyrics { get; set; }
        public virtual List<Word> Words { get; set; } = new List<Word>();
        public SongItemViewModel(Song song) : base(song)
        {
            Id = song.Id;
            Name = song.Name;
            Artist = song.Artist;
            Lyrics = song.Lyrics;
            Words = song.Words;
            StringByWhichToFind = Name;
        }
    }
}
