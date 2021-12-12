using EnglishBySongs.ViewModels.ListViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EnglishBySongs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UnlearnedWordListPage : ContentPage
    {
        private UnlearnedWordsListViewModel _wordsListViewModel;

        public UnlearnedWordListPage()
        {
            InitializeComponent();

            _wordsListViewModel = BindingContext as UnlearnedWordsListViewModel;
        }

        private void UnlearnedWordsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            _wordsListViewModel.ItemTappedCommand.Execute(e.Item);
        }

        private void UnlearnedWordsListView_LongPressing(object sender, MR.Gestures.LongPressEventArgs e)
        {
            _wordsListViewModel.DisplayCheckBoxesCommand.Execute(default(object));
        }
    }
}