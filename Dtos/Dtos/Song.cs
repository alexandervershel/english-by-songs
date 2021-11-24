using Dtos.Interfaces;
using EnglishBySongs.ViewModels;
using Entities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Dtos.Dtos
{
    // TODO: избавиться от наследования Song
    public class Song : IListViewItemViewModel, INotifyPropertyChanged, IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Lyrics { get; set; }
        public virtual List<WordModel> Words { get; set; } = new List<WordModel>();
        public string StringByWhichToFind { get; set; }
        private readonly IPageService _pageService;
        private bool _isSelected;
        public Song()
        {

        }
        public Song(IPageService pageService)
        {
            _pageService = pageService;
            StringByWhichToFind = Name;
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                SetValue(ref _isSelected, value);
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public async Task ToEditPage()
        {
            //await _pageService.PushAsync(new SongPage(new SongViewModel(this)));
            await _pageService.PushEditPageAsync<Song>();
        }

        // TODO: избавиться от этого
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetValue<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
            {
                return;
            }

            backingField = value;
            OnPropertyChanged(propertyName);
        }
    }
}
