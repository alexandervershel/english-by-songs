using EnglishBySongs.ViewModels;
using Xamarin.Forms;

namespace EnglishBySongs.Views
{
    public partial class SettingsPage : ContentPage
    {
        private SettingsViewModel _settingsViewModel;
        public SettingsPage()
        {
            InitializeComponent();

            _settingsViewModel = BindingContext as SettingsViewModel;
        }
    }
}