using Microsoft.EntityFrameworkCore;
using Entities;
using Xamarin.Forms;
using Core.Shared;

namespace Dal
{
    public class EnglishBySongsDbContext : DbContext
    {
        public const string DBFILENAME = "english_by_songs.db";
        private string _databasePath;
        private static EnglishBySongsDbContext _instance;
        public DbSet<Word> Words { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Translation> Translations { get; set; }
        public string Name { get; private set; }
        private EnglishBySongsDbContext()
        {
            _databasePath = DependencyService.Get<IDatabasePath>().GetPath(DBFILENAME);
        }

        public static EnglishBySongsDbContext GetInstance()
        {
            if (_instance == null)
                _instance = new EnglishBySongsDbContext();
            return _instance;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_databasePath}");
        }

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
