using Dtos;
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
            Comparison<WordItem> comparison = wordsSortingMode switch
            {
                WordsSortingModes.AddingDate => (w1, w2) => w1.Id.CompareTo(w2.Id),
                WordsSortingModes.AddingDateDescending => (w1, w2) => -w1.Id.CompareTo(w2.Id),
                WordsSortingModes.Foreign => (w1, w2) => w1.Foreign.CompareTo(w2.Foreign),
                WordsSortingModes.ForeignDescending => (w1, w2) => -w1.Foreign.CompareTo(w2.Foreign),
                WordsSortingModes.Translations => (w1, w2) => CompareHelper.CompareByStringValue(w1.Translations, w2.Translations),
                WordsSortingModes.TranslationsDescending => (w1, w2) => -CompareHelper.CompareByStringValue(w1.Translations, w2.Translations),
                WordsSortingModes.Songs => (w1, w2) => CompareHelper.CompareByStringValue(w1.Songs, w2.Songs),
                WordsSortingModes.SongsDescending => (w1, w2) => -CompareHelper.CompareByStringValue(w1.Songs, w2.Songs),
                _ => (w1, w2) => w1.Foreign.CompareTo(w2.Foreign),
            };
            return items.Sort(comparison);
        }

        public static ObservableCollection<SongItem> Sort(ObservableCollection<SongItem> items, SongsSortingModes songsSortingMode)
        {
            Comparison<SongItem> comparison = songsSortingMode switch
            {
                SongsSortingModes.AddingDate => (s1, s2) => s1.Id.CompareTo(s2.Id),
                SongsSortingModes.AddingDateDescending => (s1, s2) => s2.Id.CompareTo(s1.Id),
                SongsSortingModes.Name => (s1, s2) => s1.Name.CompareTo(s2.Name),
                SongsSortingModes.NameDescending => (s1, s2) => s2.Name.CompareTo(s1.Name),
                SongsSortingModes.Artist => (s1, s2) => s1.Artist.CompareTo(s2.Artist),
                SongsSortingModes.ArtistDescending => (s1, s2) => s2.Artist.CompareTo(s1.Artist),
                _ => (s1, s2) => s1.Name.CompareTo(s2.Name),
            };
            return items.Sort(comparison);
        }
    }
}
