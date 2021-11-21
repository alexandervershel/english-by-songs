using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Dal.Repositories;
using Dal;
using System;
using System.Collections.Generic;
using Core;
using Core.Extensions;
using Core.Enums;

namespace EnglishBySongs.ViewModels.ListViewModels
{
    public class SongsListViewModel : ListViewModel<SongItem>
    {
        private readonly SongRepository _songRepository = new SongRepository(EnglishBySongsDbContext.GetInstance());
        private readonly WordRepository _wordRepository = new WordRepository(EnglishBySongsDbContext.GetInstance());
        public SongsListViewModel() : base()
        {
            MessagingCenter.Subscribe<SongSearchViewModel>(
                this,
                "SongAdded",
                async (sender) =>
                {
                    await RefreshAsync();
                });

            MessagingCenter.Subscribe<ListViewModel<WordItem>>(
                this,
                "WordsListChanged",
                async (sender) =>
                {
                    await RefreshAsync();
                });

            MessagingCenter.Subscribe<ListViewModel<WordItem>>(
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
            _songRepository.GetAll().ForEach(s => Items.Add(new SongItem(s)));
        }

        protected override async Task Sort()
        {
            Comparison<SongItem> comparison;
            switch ((SongsSortingModes)Preferences.Get("SongsSortingMode", 2))
            {
                case SongsSortingModes.AddingDate:
                    comparison = (s1, s2) => s1.Id.CompareTo(s2.Id);
                    break;
                case SongsSortingModes.AddingDateDescending:
                    comparison = (s1, s2) => s2.Id.CompareTo(s1.Id);
                    break;
                case SongsSortingModes.Name:
                    comparison = (s1, s2) => s1.Name.CompareTo(s2.Name);
                    break;
                case SongsSortingModes.NameDescending:
                    comparison = (s1, s2) => s2.Name.CompareTo(s1.Name);
                    break;
                case SongsSortingModes.Artist:
                    comparison = (s1, s2) => s1.Artist.CompareTo(s2.Artist);
                    break;
                case SongsSortingModes.ArtistDescending:
                    comparison = (s1, s2) => s2.Artist.CompareTo(s1.Artist);
                    break;
                default:
                    comparison = (s1, s2) => s1.Name.CompareTo(s2.Name);
                    break;
            }
            Items.Sort(comparison);

            AllItems = Items;
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
                i.Words.Where(w => w.Songs.Count == 1 && w.Songs.First() == i).ForEach(w => _wordRepository.Remove(w));
                _songRepository.Remove(i);
            });
            _songRepository.Save();
            RefreshAsync();
            MessagingCenter.Send((ListViewModel<SongItem>)this, "SongsDeleted");
            await CancelMultiselect();
            await _pageService.DispayToast("Песни удалены");
        }
    }
}