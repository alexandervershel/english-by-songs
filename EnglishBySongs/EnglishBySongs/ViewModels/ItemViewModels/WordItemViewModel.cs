using Entities;
using System.Collections.Generic;

namespace EnglishBySongs.ViewModels.ItemViewModels
{
    public class WordItemViewModel : BaseItemViewModel<Word>
    {
        public int Id { get; set; }
        public string Foreign { get; set; }
        public bool IsLearned { get; set; }
        public virtual ICollection<Translation> Translations { get; set; }
        public virtual ICollection<Song> Songs { get; set; }
        public WordItemViewModel(Word word) : base(word)
        {
            Id = word.Id;
            Foreign = word.Foreign;
            IsLearned = word.IsLearned;
            Translations = word.Translations;
            Songs = word.Songs;
            StringByWhichToFind = word.Foreign;
        }
    }
}
