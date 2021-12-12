using EnglishBySongs.ViewModels;
using Xamarin.Forms;

namespace EnglishBySongs.Views
{
    public partial class SongSearchPage : ContentPage
    {
        private readonly SongSearchViewModel _SongSearchPageViewModel;
        public SongSearchPage()
        {
            InitializeComponent();

            _SongSearchPageViewModel = BindingContext as SongSearchViewModel;
        }
    }
}