using Dal;
using Xamarin.Forms;

namespace EnglishBySongs
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //EnglishBySongsDbContext.GetInstance().Database.EnsureDeleted();
            EnglishBySongsDbContext.GetInstance().Database.EnsureCreated();

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
