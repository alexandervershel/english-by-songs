using EnglishBySongs.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EnglishBySongs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WordsAddingPage : ContentPage
    {

        public WordsAddingPage(WordsAddingBySongPageViewModel wordsAddingBySongPageViewModel)
        {
            InitializeComponent();

            BindingContext = wordsAddingBySongPageViewModel;
        }
    }
}