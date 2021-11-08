using EnglishBySongs.Models;
using EnglishBySongs.Services;
using EnglishBySongs.Views;
using System.Threading.Tasks;

namespace EnglishBySongs.ViewModels
{
    public class SongItem : Song, ISelectable, IEditable, ISearchable
    {
        private IPageService _pageService;

        public SongItem(Song song)
        {
            _pageService = new PageService();

            Id = song.Id;
            Name = song.Name;
            Artist = song.Artist;
            Lyrics = song.Lyrics;
            Words = song.Words;
            stringByWhichToFind = Name;
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                SetValue(ref _isSelected, value);
                OnPropertyChanged(nameof(IsSelected));
            }
        }
        public string stringByWhichToFind { get; set; }

        public async Task ToEditPage()
        {
            await _pageService.PushAsync(new SongPage(new SongViewModel(this)));
        }
    }
}
