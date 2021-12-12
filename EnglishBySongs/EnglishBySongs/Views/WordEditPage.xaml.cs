using EnglishBySongs.ViewModels.EditViewModels;
using Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EnglishBySongs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WordEditPage : ContentPage
    {
        private WordEditViewModel _wordViewModel;

        public WordEditPage(WordEditViewModel wordViewModel)
        {
            InitializeComponent();

            _wordViewModel = wordViewModel;
            BindingContext = _wordViewModel;
        }

        private void RemoveItemImageButton_Clicked(object sender, System.EventArgs e)
        {
            var button = sender as ImageButton;
            var translation = button.BindingContext as Translation;
            _wordViewModel.RemoveTranslationCommand.Execute(translation);
        }
    }
}