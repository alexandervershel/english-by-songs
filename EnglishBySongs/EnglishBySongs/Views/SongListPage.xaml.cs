using EnglishBySongs.ViewModels.ListViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EnglishBySongs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SongListPage : ContentPage
    {
        private readonly SongListViewModel _songsListViewModel;

        public SongListPage()
        {
            InitializeComponent();

            _songsListViewModel = BindingContext as SongListViewModel;
        }

        private void SongsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            _songsListViewModel.ItemTappedCommand.Execute(e.Item);
        }

        private void SongsListView_LongPressing(object sender, MR.Gestures.LongPressEventArgs e)
        {
            _songsListViewModel.DisplayCheckBoxesCommand.Execute(default(object));
        }
    }
}