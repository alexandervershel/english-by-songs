﻿namespace EnglishBySongs.ViewModels
{
    public interface IListViewItemViewModel
    {
        public bool IsSelected { get; set; }
        public string StringByWhichToFind { get; set; }
    }
}
