using EnglishBySongs.Data;
using EnglishBySongs.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace EnglishBySongs.ViewModels
{
    public class SongsListViewModel : ListViewModel<SongItem>
    {
        public SongsListViewModel() : base()
        {
            MessagingCenter.Subscribe<WordsAddingBySongPageViewModel>(
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
        }

        protected override async Task ReadCollectionFromDb()
        {
            Items.Clear();
            List<Song> songs;

            using (EnglishBySongsDbContext db = new EnglishBySongsDbContext())
            {
                songs = await db.Songs.Include(w => w.Words).ToListAsync();
            }

            foreach (var song in songs)
            {
                Items.Add(new SongItem(song));
            }
        }

        protected override async Task DeleteItems(object obj)
        {
            if (SelectedItems.Count == 0)
                return;

            bool isConfirmed = await _pageService.DisplayAlert("Вы действительно хотите удалить выбранные песни?", $"Количество выбранных песен: {SelectedItems.Count}", "да", "нет");
            if (!isConfirmed)
            {
                return;
            }

            using (EnglishBySongsDbContext db = new EnglishBySongsDbContext())
            {
                SelectedItems.ForEach(i => db.Songs.Remove(db.Songs.Find(i.Id)));
                db.SaveChanges();
            }

            await RefreshAsync();
            MessagingCenter.Send((ListViewModel<SongItem>)this, "SongsDeleted");

            await CancelMultiselect();
        }
    }
}