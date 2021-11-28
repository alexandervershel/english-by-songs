using EnglishBySongs.ViewModels.Items;
using Services.Enums;
using Services.Extensions;
using System;
using System.Collections.ObjectModel;

namespace EnglishBySongs.Helpers
{
    internal static class SortHelper
    {
        public static ObservableCollection<WordItem> Sort(ObservableCollection<WordItem> items, WordsSortingModes wordsSortingMode)
        {
            Comparison<WordItem> comparison;
            switch (wordsSortingMode)
            {
                case WordsSortingModes.AddingDate:
                    comparison = (w1, w2) => w1.Id.CompareTo(w2.Id);
                    break;
                case WordsSortingModes.AddingDateDescending:
                    comparison = (w1, w2) => -w1.Id.CompareTo(w2.Id);
                    break;
                case WordsSortingModes.Foreign:
                    comparison = (w1, w2) => w1.Foreign.CompareTo(w2.Foreign);
                    break;
                case WordsSortingModes.ForeignDescending:
                    comparison = (w1, w2) => -w1.Foreign.CompareTo(w2.Foreign);
                    break;
                case WordsSortingModes.Translations:
                    comparison = (w1, w2) => CompareHelper.CompareByStringValue(w1.Translations, w2.Translations);
                    break;
                case WordsSortingModes.TranslationsDescending:
                    comparison = (w1, w2) => -CompareHelper.CompareByStringValue(w1.Translations, w2.Translations);
                    break;
                case WordsSortingModes.Songs:
                    comparison = (w1, w2) => CompareHelper.CompareByStringValue(w1.Songs, w2.Songs);
                    break;
                case WordsSortingModes.SongsDescending:
                    comparison = (w1, w2) => -CompareHelper.CompareByStringValue(w1.Songs, w2.Songs);
                    break;
                default:
                    comparison = (w1, w2) => w1.Foreign.CompareTo(w2.Foreign);
                    break;
            }
            return items.Sort(comparison);
        }

        public static ObservableCollection<SongItem> Sort(ObservableCollection<SongItem> items, SongsSortingModes songsSortingMode)
        {
            Comparison<SongItem> comparison;
            switch (songsSortingMode)
            {
                case SongsSortingModes.AddingDate:
                    comparison = (s1, s2) => s1.Id.CompareTo(s2.Id);
                    break;
                case SongsSortingModes.AddingDateDescending:
                    comparison = (s1, s2) => s2.Id.CompareTo(s1.Id);
                    break;
                case SongsSortingModes.Name:
                    comparison = (s1, s2) => s1.Name.CompareTo(s2.Name);
                    break;
                case SongsSortingModes.NameDescending:
                    comparison = (s1, s2) => s2.Name.CompareTo(s1.Name);
                    break;
                case SongsSortingModes.Artist:
                    comparison = (s1, s2) => s1.Artist.CompareTo(s2.Artist);
                    break;
                case SongsSortingModes.ArtistDescending:
                    comparison = (s1, s2) => s2.Artist.CompareTo(s1.Artist);
                    break;
                default:
                    comparison = (s1, s2) => s1.Name.CompareTo(s2.Name);
                    break;
            }
            return items.Sort(comparison);
        }
    }
}
