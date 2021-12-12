using EnglishBySongs.ViewModels.ListViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EnglishBySongs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LearnedWordListPage : ContentPage
    {
        private LearnedWordsListViewModel _wordsListViewModel;

        public LearnedWordListPage()
        {
            InitializeComponent();

            _wordsListViewModel = BindingContext as LearnedWordsListViewModel;
        }

        private void LearnedWordsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            _wordsListViewModel.ItemTappedCommand.Execute(e.Item);
        }

        private void LearnedWordsListView_LongPressing(object sender, MR.Gestures.LongPressEventArgs e)
        {
            _wordsListViewModel.DisplayCheckBoxesCommand.Execute(default(object));
        }
    }
}