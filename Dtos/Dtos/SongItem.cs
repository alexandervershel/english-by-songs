using EnglishBySongs.ViewModels.EditViewModels;
using EnglishBySongs.Views;
using Entities;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ViewModels.EditViewModels;

namespace ViewModels.Dtos
{
    // TODO: избавиться от наследования Song
    public class SongItem : IListViewItemViewModel, INotifyPropertyChanged
    {
        private static readonly IServiceProvider _serviceProvider = ServiceProviderFactory.ServiceProvider;
        private readonly IPageService _pageService;
        public int Id { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Lyrics { get; set; }
        public virtual List<Word> Words { get; set; } = new List<Word>();
        // TODO: по возможности удалить это свойство из модели
        //[NotMapped]
        public ICollection<Word> UnlearnedWords
        {
            get
            {
                return Words.Where(w => !w.IsLearned).ToList();
            }
        }

        // TODO: это свойство тоже надо удалить
        //[NotMapped]
        public ICollection<Word> LearnedWords
        {
            get
            {
                return Words.Where(w => w.IsLearned).ToList();
            }
        }

        public override string ToString()
        {
            return $"{Artist} - {Name}";
        }
        public SongItem(Song song)
        {
            _pageService = _serviceProvider.GetService<IPageService>();

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

        public async Task ToEditPage()
        {
            await _pageService.PushAsync(new SongPage(new SongViewModel(this)));
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
