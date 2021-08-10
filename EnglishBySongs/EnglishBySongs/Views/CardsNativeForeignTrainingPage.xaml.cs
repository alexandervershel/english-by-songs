using EnglishBySongs.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EnglishBySongs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardsNativeForeignTrainingPage : ContentPage
    {
        private TapGestureRecognizer _cardTapGestureRecognizer;

        private TrainingViewModel _trainingViewModel;

        public CardsNativeForeignTrainingPage()
        {
            InitializeComponent();

            _cardTapGestureRecognizer = new TapGestureRecognizer();
            _trainingViewModel = BindingContext as TrainingViewModel;
            _cardTapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Frame frame = (Frame)sender;
            Grid grid = (Grid)frame.Content;
            frame.GestureRecognizers.Clear();

            if (frame.RotationY % 360 == 0)
            {
                grid.Children[1].IsVisible = true;
                grid.Children[0].IsVisible = false;
            }
            else
            {
                grid.Children[1].IsVisible = false;
                grid.Children[0].IsVisible = true;
            }
            await frame.RotateYTo((frame.RotationY + 180) % 360, 180, Easing.SpringOut);

            frame.GestureRecognizers.Add(_cardTapGestureRecognizer);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (WordsCarouselView.Position != _trainingViewModel.Words.Count - 1)
            {
                WordsCarouselView.Position += 1;
            }
            else
            {
                _trainingViewModel.EndTrainingCommand.Execute(null);
            }
        }
    }
}