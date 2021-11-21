using Core.Enums;
using Dal;
using Dal.Repositories;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace EnglishBySongs.ViewModels.ListViewModels
{
    public class UnlearnedWordsListViewModel : ListViewModel<WordItem>
    {
        public ICommand TransferToLearnedWordsCommand { get; private set; }
        private readonly WordRepository _wordRepository = new WordRepository(EnglishBySongsDbContext.GetInstance());
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

            MessagingCenter.Subscribe<ListViewModel<SongItem>>(
                this,
                "SongsDeleted",
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

            MessagingCenter.Subscribe<WordViewModel>(
                this,
                "WordUpdated",
                async (sender) =>
                {
                    await RefreshAsync();
                });
        }

        protected override void ReadCollectionFromDb()
        {
            Items.Clear();
            _wordRepository.GetAll(w => !w.IsLearned).ForEach(w => Items.Add(new WordItem(w)));
        }

        protected override async Task Sort()
        {
            IEnumerable<WordItem> words;
            switch ((WordsSortingModes)Preferences.Get("WordsSortingMode", 2))
            {
                case WordsSortingModes.AddingDate:
                    words = Items.OrderBy(x => x.Id);
                    break;
                case WordsSortingModes.AddingDateDescending:
                    words = Items.OrderByDescending(x => x.Id);
                    break;
                case WordsSortingModes.Foreign:
                    words = Items.OrderBy(x => x.Foreign);
                    break;
                case WordsSortingModes.ForeignDescending:
                    words = Items.OrderByDescending(x => x.Foreign);
                    break;
                case WordsSortingModes.Translations:
                    words = Items.OrderBy(x => string.Join("", x.Translations)).OrderByDescending(x => x.Translations?.Any());
                    break;
                case WordsSortingModes.TranslationsDescending:
                    words = Items.OrderByDescending(x => string.Join("", x.Translations)).OrderByDescending(x => x.Translations?.Any());
                    break;
                case WordsSortingModes.Songs:
                    words = Items.OrderBy(x => string.Join("", x.Songs)).OrderByDescending(x => x.Songs?.Any());
                    break;
                case WordsSortingModes.SongsDescending:
                    words = Items.OrderByDescending(x => string.Join("", x.Songs)).OrderByDescending(x => x.Songs?.Any());
                    break;
                default:
                    words = Items.OrderBy(x => x.Foreign);
                    break;
            }
            Items = new ObservableCollection<WordItem>(words);
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

            SelectedItems.ForEach(i => _wordRepository.Remove(i));
            _wordRepository.Save();
            MessagingCenter.Send((ListViewModel<WordItem>)this, "WordsListChanged");
            await CancelMultiselect();
            await _pageService.DispayToast("Слова удалены");
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

            SelectedItems.ForEach(i => { i.IsLearned = true; _wordRepository.Update(i); });
            _wordRepository.Save();
            MessagingCenter.Send((ListViewModel<WordItem>)this, "WordsListChanged");
            await CancelMultiselect();
            await _pageService.DispayToast("Слова перенесены в раздел \"НЕВЫУЧЕНО\"");
        }
    }
}
