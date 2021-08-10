using System;
using System.Collections.Generic;
using EnglishBySongs.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EnglishBySongs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LyricsPage : ContentPage
    {
        public LyricsPage(WordsAddingBySongPageViewModel wordsAddingBySongPageViewModel)
        {
            InitializeComponent();

            BindingContext = wordsAddingBySongPageViewModel;
        }
    }
}