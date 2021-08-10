using EnglishBySongs.Models;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;

namespace EnglishBySongs.Data
{
    class EnglishBySongsDbContext : DbContext
    {
        public const string DBFILENAME = "english_by_songs.db";

        private string _databasePath;

        public EnglishBySongsDbContext()
        {
            _databasePath = DependencyService.Get<IDatabasePath>().GetPath(DBFILENAME);
        }

        public DbSet<Word> Words { get; set; }

        public DbSet<Song> Songs { get; set; }

        public DbSet<Translation> Translations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Filename={_databasePath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Song>()
                .HasMany(s => s.Words)
                .WithMany(w => w.Songs);

            modelBuilder.Entity<Word>()
                .HasMany(w => w.Translations)
                .WithMany(t => t.Words);
        }
    }
}
