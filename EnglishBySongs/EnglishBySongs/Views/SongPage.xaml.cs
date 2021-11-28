using EnglishBySongs.ViewModels.EditViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EnglishBySongs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SongPage : ContentPage
    {
        private SongViewModel _songViewModel;

        public SongPage(SongViewModel songViewModel)
        {
            InitializeComponent();

            _songViewModel = songViewModel;
            BindingContext = _songViewModel;
        }
    }
}