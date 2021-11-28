using Entities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EnglishBySongs.ViewModels.Items
{
    // TODO: избавиться от наследования Song
    public class SongItem : Song, IListViewItemViewModel, INotifyPropertyChanged
    {
        public SongItem(Song song)
        {
            Id = song.Id;
            Name = song.Name;
            Artist = song.Artist;
            Lyrics = song.Lyrics;
            Words = song.Words;
            StringByWhichToFind = Name;
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
        public string StringByWhichToFind { get; set; }

        // TODO: избавиться от этого
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetValue<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
                return;

            backingField = value;

            OnPropertyChanged(propertyName);
        }
    }
}
