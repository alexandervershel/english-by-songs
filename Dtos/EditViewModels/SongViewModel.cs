using EnglishBySongs.ViewModels.Items;

namespace ViewModels.EditViewModels
{
    public class SongViewModel : BaseViewModel
    {
        public SongViewModel()
        {
        }

        public SongViewModel(SongItem song) : this()
        {
            _primarySong = song;
            Artist = song.Artist;
            Name = song.Name;
        }

        private SongItem _primarySong;

        public SongItem PrimarySong
        {
            get { return _primarySong; }
            set
            {
                SetValue(ref _primarySong, value);
                OnPropertyChanged(nameof(PrimarySong));
            }
        }

        private object _artist;

        public object Artist
        {
            get { return _artist; }
            set
            {
                SetValue(ref _artist, value);
                OnPropertyChanged(nameof(Artist));
            }
        }

        private object _name;

        public object Name
        {
            get { return _name; }
            set
            {
                SetValue(ref _name, value);
                OnPropertyChanged(nameof(Name));
            }
        }
    }
}
