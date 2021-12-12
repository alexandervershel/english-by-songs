using EnglishBySongs.ViewModels.EditViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EnglishBySongs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SongEditPage : ContentPage
    {
        private SongEditViewModel _songViewModel;

        public SongEditPage(SongEditViewModel songViewModel)
        {
            InitializeComponent();

            _songViewModel = songViewModel;
            BindingContext = _songViewModel;
        }
    }
}