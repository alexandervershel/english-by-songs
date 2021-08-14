using EnglishBySongs.Data;
using EnglishBySongs.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace EnglishBySongs.ViewModels
{
    public class LearnedWordsListViewModel : ListViewModel<WordItem>
    {
        public LearnedWordsListViewModel() : base()
        {
            TransferToUnlearnedWordsCommand = new Command(async () => await TransferToUnlearnedWords());

            MessagingCenter.Subscribe<WordsAddingBySongPageViewModel>(
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
        }

        public ICommand TransferToUnlearnedWordsCommand { get; private set; }

        protected override async Task ReadCollectionFromDb()
        {
            Items.Clear();
            List<Word> words;

            using (EnglishBySongsDbContext db = new EnglishBySongsDbContext())
            {
                words = await db.Words.Include(w => w.Songs).Include(w => w.Translations).Where(w => w.IsLearned).ToListAsync();
            }

            foreach (var word in words)
            {
                Items.Add(new WordItem(word));
            }
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
                return;

            bool isConfirmed = await _pageService.DisplayAlert("Вы действительно хотите удалить выбранные слова?", $"Количество выбранных слов: {SelectedItems.Count}", "да", "нет");
            if (!isConfirmed)
            {
                return;
            }

            using (EnglishBySongsDbContext db = new EnglishBySongsDbContext())
            {
                SelectedItems.ForEach(i => db.Words.Remove(db.Words.Find(i.Id)));
                db.SaveChanges();
            }

            MessagingCenter.Send((ListViewModel<WordItem>)this, "WordsListChanged");

            await CancelMultiselect();
            await _pageService.DispayToast("Слова удалены");
        }

        private async Task TransferToUnlearnedWords()
        {
            if (SelectedItems.Count == 0)
                return;

            bool isConfirmed = await _pageService.DisplayAlert("Вы действительно хотите отметить выбранные слова как выученные? Слова будут перенесены в раздел \"ВЫУЧЕНО\"", $"Количество выбранных слов: {SelectedItems.Count}", "да", "нет");
            if (!isConfirmed)
            {
                return;
            }

            using (EnglishBySongsDbContext db = new EnglishBySongsDbContext())
            {
                SelectedItems.ForEach(i => db.Words.Find(i.Id).IsLearned = false);
                db.SaveChanges();
            }

            MessagingCenter.Send((ListViewModel<WordItem>)this, "WordsListChanged");

            await CancelMultiselect();
            await _pageService.DispayToast("Слова перенесены в раздел \"ВЫУЧЕНО\"");
        }
    }
}
