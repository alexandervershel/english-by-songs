using EnglishBySongs.ViewModels;
using Xamarin.Forms;

namespace EnglishBySongs.Views
{
    public partial class WordsAddingBySongPage : ContentPage
    {
        private WordsAddingBySongPageViewModel _wordsAddingBySongPageViewModel;
        public WordsAddingBySongPage()
        {
            InitializeComponent();

            _wordsAddingBySongPageViewModel = BindingContext as WordsAddingBySongPageViewModel;
            //BindingContext = _wordsAddingBySongPageViewModel;
        }
    }
}