using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System;
using Services.Enums;
using Services.Interfaces;
using Entities;
using Services;
using Microsoft.Extensions.DependencyInjection;
using EnglishBySongs.Helpers;
using EnglishBySongs.Views;
using EnglishBySongs.ViewModels.EditViewModels;
using EnglishBySongs.ViewModels.ItemViewModels;

namespace EnglishBySongs.ViewModels.ListViewModels
{
    public class SongListViewModel : BaseListViewModel<SongItemViewModel, Song>
    {
        private static readonly IServiceProvider _serviceProvider = ServiceProviderFactory.ServiceProvider;
        private readonly IRepository<Song> _songRepository = _serviceProvider.GetService<IRepository<Song>>();
        private readonly IRepository<Word> _wordRepository = _serviceProvider.GetService<IRepository<Word>>();
        public SongListViewModel() : base()
        {
            MessagingCenter.Subscribe<SongSearchViewModel>(
                this,
                "SongAdded",
                async (sender) =>
                {
                    await RefreshAsync();
                });

            MessagingCenter.Subscribe<BaseListViewModel<WordItemViewModel, Word>>(
                this,
                "WordsListChanged",
                async (sender) =>
                {
                    await RefreshAsync();
                });

            MessagingCenter.Subscribe<BaseListViewModel<WordItemViewModel, Word>>(
                this,
                "SongsDeleted",
                async (sender) =>
                {
                    await RefreshAsync();
                });

            MessagingCenter.Subscribe<SettingsViewModel>(
                this,
                "SongsSortingModeChanged",
                async (sender) =>
                {
                    await Sort();
                });
        }

        protected override void ReadCollectionFromDb()
        {
            Items.Clear();
            _songRepository.GetAll().ForEach(s => Items.Add(new SongItemViewModel(s)));
        }

        protected override async Task Sort()
        {
            Items = SortHelper.Sort(Items, (SongsSortingModes)Preferences.Get("SongsSortingMode", (int)SongsSortingModes.Name));
            AllItems = Items;
        }

        protected override async Task ToItemEditPage()
        {
            await _pageService.PushAsync(new SongEditPage(new SongEditViewModel(SelectedItem.Model)));
        }

        protected override async Task DeleteItems(object obj)
        {
            if (SelectedItems.Count == 0)
            {
                return;
            }

            bool isConfirmed = await _pageService.DisplayAlert("Вы действительно хотите удалить выбранные песни? Слова из песни также будут удалены", $"Количество выбранных песен: {SelectedItems.Count}", "да", "нет");
            if (!isConfirmed)
            {
                return;
            }
            
            SelectedItems.ForEach(i =>
            {
                i.Words.Where(w => w.Songs.Count == 1 && w.Songs.First() == i.Model).ForEach(w => _wordRepository.Remove(w));
                _songRepository.Remove(i.Model);
            });
            _songRepository.Save();
            RefreshAsync();
            MessagingCenter.Send((BaseListViewModel<SongItemViewModel, Song>)this, "SongsDeleted");
            await DisableMultiselect();
            await _pageService.DispayToast("Песни удалены");
        }
    }
}