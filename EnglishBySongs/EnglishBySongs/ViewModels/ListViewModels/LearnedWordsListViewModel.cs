using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Services.Enums;
using Services.Interfaces;
using Entities;
using System;
using Services;
using Microsoft.Extensions.DependencyInjection;
using EnglishBySongs.ViewModels.Items;
using EnglishBySongs.Helpers;
using EnglishBySongs.ViewModels.EditViewModels;

namespace EnglishBySongs.ViewModels.ListViewModels
{
    public class LearnedWordsListViewModel : BaseListViewModel<WordItem>
    {
        private static readonly IServiceProvider _serviceProvider = ServiceProviderFactory.ServiceProvider;
        public ICommand TransferToUnlearnedWordsCommand { get; private set; }
        private readonly IRepository<Word> _wordRepository = _serviceProvider.GetService<IRepository<Word>>();
        public LearnedWordsListViewModel() : base()
        {
            TransferToUnlearnedWordsCommand = new Command(async () => await TransferToUnlearnedWords());

            MessagingCenter.Subscribe<SongSearchViewModel>(
                this,
                "WordsAdded",
                async (sender) =>
                {
                    await RefreshAsync();
                });

            MessagingCenter.Subscribe<WordViewModel>(
                this,
                "WordUpdated",
                async (sender) =>
                {
                    await RefreshAsync();
                });

            MessagingCenter.Subscribe<BaseListViewModel<WordItem>>(
                this,
                "WordsListChanged",
                async (sender) =>
                {
                    await RefreshAsync();
                });

            MessagingCenter.Subscribe<SettingsViewModel>(
                this,
                "WordsSortingModeChanged",
                async (sender) =>
                {
                    await Sort();
                });

            MessagingCenter.Subscribe<BaseListViewModel<SongItem>>(
                this,
                "SongsDeleted",
                async (sender) =>
                {
                    await RefreshAsync();
                });
        }

        protected override void ReadCollectionFromDb()
        {
            Items.Clear();
            _wordRepository.GetAll(w => w.IsLearned).ForEach(w => Items.Add(new WordItem(w)));
        }

        protected override async Task Sort()
        {
            Items = SortHelper.Sort(Items, (WordsSortingModes)Preferences.Get("WordsSortingMode", (int)WordsSortingModes.Foreign));
            AllItems = Items;
        }

        protected override async Task DeleteItems(object obj)
        {
            if (SelectedItems.Count == 0)
            {
                return;
            }

            bool isConfirmed = await _pageService.DisplayAlert("Вы действительно хотите удалить выбранные слова?", $"Количество выбранных слов: {SelectedItems.Count}", "да", "нет");
            if (!isConfirmed)
            {
                return;
            }

            SelectedItems.ForEach(i => _wordRepository.Remove(i));
            _wordRepository.Save();
            MessagingCenter.Send((BaseListViewModel<WordItem>)this, "WordsListChanged");
            await DisableMultiselect();
            await _pageService.DispayToast("Слова удалены");
        }

        private async Task TransferToUnlearnedWords()
        {
            if (SelectedItems.Count == 0)
            {
                return;
            }

            bool isConfirmed = await _pageService.DisplayAlert("Вы действительно хотите отметить выбранные слова как выученные? Слова будут перенесены в раздел \"ВЫУЧЕНО\"", $"Количество выбранных слов: {SelectedItems.Count}", "да", "нет");
            if (!isConfirmed)
            {
                return;
            }

            SelectedItems.ForEach(i => { i.IsLearned = false; _wordRepository.Update(i); });
            _wordRepository.Save();
            MessagingCenter.Send((BaseListViewModel<WordItem>)this, "WordsListChanged");
            await DisableMultiselect();
            await _pageService.DispayToast("Слова перенесены в раздел \"ВЫУЧЕНО\"");
        }
    }
}
