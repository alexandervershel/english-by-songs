using Services.Enums;
using Entities;
using Services.Interfaces;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System;
using Services;
using Microsoft.Extensions.DependencyInjection;
using EnglishBySongs.Helpers;
using EnglishBySongs.ViewModels.EditViewModels;
using EnglishBySongs.Views;
using EnglishBySongs.ViewModels.ItemViewModels;

namespace EnglishBySongs.ViewModels.ListViewModels
{
    public class UnlearnedWordsListViewModel : BaseListViewModel<WordItemViewModel, Word>
    {
        private static readonly IServiceProvider _serviceProvider = ServiceProviderFactory.ServiceProvider;
        public ICommand TransferToLearnedWordsCommand { get; private set; }
        private readonly IRepository<Word> _wordRepository = _serviceProvider.GetService<IRepository<Word>>();
        public UnlearnedWordsListViewModel() : base()
        {
            TransferToLearnedWordsCommand = new Command(async () => await TransferToLearnedWords());

            MessagingCenter.Subscribe<SongSearchViewModel>(
                this,
                "WordsAdded",
                async (sender) =>
                {
                    await RefreshAsync();
                });

            MessagingCenter.Subscribe<BaseListViewModel<SongItemViewModel, Song>>(
                this,
                "SongsDeleted",
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

            MessagingCenter.Subscribe<SettingsViewModel>(
                this,
                "WordsSortingModeChanged",
                async (sender) =>
                {
                    await Sort();
                });

            MessagingCenter.Subscribe<WordEditViewModel>(
                this,
                "WordUpdated",
                async (sender) =>
                {
                    await RefreshAsync();
                });
        }

        protected override async Task Sort()
        {
            Items = SortHelper.Sort(Items, (WordsSortingModes)Preferences.Get("WordsSortingMode", (int)WordsSortingModes.Foreign));
            AllItems = Items;
        }

        protected override async Task DeleteItems(object obj)
        {
            if (SelectedItems.Count == 0)
                return;

            bool isConfirmed = await _pageService.DisplayAlert("Вы действительно хотите удалить выбранные слова?", $"Количество выбранных слов: {SelectedItems.Count}", "да", "нет");
            if (!isConfirmed)
            {
                return;
            }

            SelectedItems.ForEach(i => _wordRepository.Remove(i.Model));
            _wordRepository.Save();
            MessagingCenter.Send((BaseListViewModel<WordItemViewModel, Word>)this, "WordsListChanged");
            await DisableMultiselect();
            await _pageService.DispayToast("Слова удалены");
        }

        protected override void ReadCollectionFromDb()
        {
            Items.Clear();
            _wordRepository.GetAll(w => !w.IsLearned).ForEach(w => Items.Add(new WordItemViewModel(w)));
        }

        protected override async Task ToItemEditPage()
        {
            await _pageService.PushAsync(new WordEditPage(new WordEditViewModel(SelectedItem.Model)));
        }

        private async Task TransferToLearnedWords()
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

            SelectedItems.ForEach(i => { i.IsLearned = true; _wordRepository.Update(i.Model); });
            _wordRepository.Save();
            MessagingCenter.Send((BaseListViewModel<WordItemViewModel, Word>)this, "WordsListChanged");
            await DisableMultiselect();
            await _pageService.DispayToast("Слова перенесены в раздел \"НЕВЫУЧЕНО\"");
        }
    }
}
