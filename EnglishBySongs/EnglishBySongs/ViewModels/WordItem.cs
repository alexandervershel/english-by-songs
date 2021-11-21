using EnglishBySongs.Services;
using EnglishBySongs.Views;
using Entities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace EnglishBySongs.ViewModels
{
    public class WordItem : Word, IListViewItemViewModel, INotifyPropertyChanged
    {
        private IPageService _pageService;

        //public WordItem()
        //{
        //    _pageService = new PageService();
        //    stringByWhichToFind = Foreign;
        //}
        public WordItem()
        {

        }

        public WordItem(Word word)// : this()
        {
            _pageService = new PageService();

            Id = word.Id;
            Foreign = word.Foreign;
            IsLearned = word.IsLearned;
            Translations = word.Translations;
            Songs = word.Songs;

            StringByWhichToFind = word.Foreign;
        }

        // TODO: перенести в базовый класс
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

        public async Task ToEditPage()
        {
            await _pageService.PushAsync(new WordPage(new WordViewModel(this)));
        }

        // TODO: удалить
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
