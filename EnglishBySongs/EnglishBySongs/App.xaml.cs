using Services;
using Xamarin.Forms;

namespace EnglishBySongs
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
            var serviceProvider = ServiceProviderFactory.ServiceProvider;
            //ServiceProviderFactory.Context.Database.EnsureDeleted();
            ServiceProviderFactory.Context.Database.EnsureCreated();
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
