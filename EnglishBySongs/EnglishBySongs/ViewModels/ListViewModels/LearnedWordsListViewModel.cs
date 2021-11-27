using System.Collections.ObjectModel;
using System.Linq;
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

namespace EnglishBySongs.ViewModels.ListViewModels
{
    public class LearnedWordsListViewModel : ListViewModel<WordItem>
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

            MessagingCenter.Subscribe<ListViewModel<WordItem>>(
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

            MessagingCenter.Subscribe<ListViewModel<SongItem>>(
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
            switch ((WordsSortingModes)Preferences.Get("WordsSortingMode", 2))
            {
                case WordsSortingModes.AddingDate:
                    Items = new ObservableCollection<WordItem>(Items.OrderBy(x => x.Id));
                    break;
                case WordsSortingModes.AddingDateDescending:
                    Items = new ObservableCollection<WordItem>(Items.OrderByDescending(x => x.Id));
                    break;
                case WordsSortingModes.Foreign:
                    Items = new ObservableCollection<WordItem>(Items.OrderBy(x => x.Foreign));
                    break;
                case WordsSortingModes.ForeignDescending:
                    Items = new ObservableCollection<WordItem>(Items.OrderByDescending(x => x.Foreign));
                    break;
                case WordsSortingModes.Translations:
                    Items = new ObservableCollection<WordItem>(Items.OrderBy(x => string.Join("", x.Translations)).OrderByDescending(x => x.Translations?.Any()));
                    break;
                case WordsSortingModes.TranslationsDescending:
                    Items = new ObservableCollection<WordItem>(Items.OrderByDescending(x => string.Join("", x.Translations)).OrderByDescending(x => x.Translations?.Any()));
                    break;
                case WordsSortingModes.Songs:
                    Items = new ObservableCollection<WordItem>(Items.OrderBy(x => string.Join("", x.Songs)).OrderByDescending(x => x.Songs?.Any()));
                    break;
                case WordsSortingModes.SongsDescending:
                    Items = new ObservableCollection<WordItem>(Items.OrderByDescending(x => string.Join("", x.Songs)).OrderByDescending(x => x.Songs?.Any()));
                    break;
                default:
                    Items = new ObservableCollection<WordItem>(Items.OrderBy(x => x.Foreign));
                    break;
            }
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
            MessagingCenter.Send((ListViewModel<WordItem>)this, "WordsListChanged");
            await CancelMultiselect();
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
            MessagingCenter.Send((ListViewModel<WordItem>)this, "WordsListChanged");
            await CancelMultiselect();
            await _pageService.DispayToast("Слова перенесены в раздел \"ВЫУЧЕНО\"");
        }
    }
}
