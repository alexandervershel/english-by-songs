using EnglishBySongs.ViewModels.EditViewModels;
using EnglishBySongs.Views;
using Entities;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace EnglishBySongs.ViewModels.Items
{
    public class WordItem : Word, IListViewItemViewModel, INotifyPropertyChanged
    {
        private static readonly IServiceProvider _serviceProvider = ServiceProviderFactory.ServiceProvider;
        private readonly IPageService _pageService;

        public WordItem(Word word)
        {
            _pageService = _serviceProvider.GetService<IPageService>();

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
