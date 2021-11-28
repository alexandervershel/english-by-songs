using Entities;

namespace EnglishBySongs.ViewModels.EditViewModels
{
    public class SongViewModel : BaseViewModel
    {
        public SongViewModel()
        {
        }

        public SongViewModel(Song song) : this()
        {
            _primarySong = song;
            Artist = song.Artist;
            Name = song.Name;
        }

        private Song _primarySong;

        public Song PrimarySong
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
