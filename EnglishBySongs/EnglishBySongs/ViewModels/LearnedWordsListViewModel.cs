using EnglishBySongs.Data;
using EnglishBySongs.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace EnglishBySongs.ViewModels
{
    public class LearnedWordsListViewModel : ListViewModel<WordItem>
    {
        public LearnedWordsListViewModel() : base()
        {
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
        }

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

            await RefreshAsync();
            MessagingCenter.Send((ListViewModel<WordItem>)this, "WordsListChanged");

            await CancelMultiselect();
        }
    }
}
