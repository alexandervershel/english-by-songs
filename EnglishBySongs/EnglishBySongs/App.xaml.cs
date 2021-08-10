using EnglishBySongs.Data;
using Xamarin.Forms;

namespace EnglishBySongs
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            using (var db = new EnglishBySongsDbContext())
            {
                //db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
