using EnglishBySongs.ViewModels;
using Xamarin.Forms;

namespace EnglishBySongs.Views
{
    public partial class StartTrainingPage : ContentPage
    {
        private TrainingsMenuViewModel _trainingsMenuViewModel;
        public StartTrainingPage()
        {
            InitializeComponent();

            _trainingsMenuViewModel = BindingContext as TrainingsMenuViewModel;
        }

        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            DisplayAlert("dsf", "bruh", "df");
        }
    }
}