﻿using EnglishBySongs.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EnglishBySongs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SongsListPage : ContentPage
    {
        private SongsListViewModel _songsListViewModel;

        public SongsListPage()
        {
            InitializeComponent();

            _songsListViewModel = BindingContext as SongsListViewModel;
        }

        private async void SongsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            _songsListViewModel.ItemTappedCommand.Execute(e.Item);
        }

        private void SongsListView_LongPressing(object sender, MR.Gestures.LongPressEventArgs e)
        {
            _songsListViewModel.DisplayCheckBoxesCommand.Execute(default(object));
        }
    }
}