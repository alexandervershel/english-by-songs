using Entities;
using System.Collections.Generic;

namespace Dtos
{
    internal class Word
    {
        public int Id { get; set; }
        public string Foreign { get; set; }
        public bool IsLearned { get; set; }
        public virtual ICollection<TranslationModel> Translations { get; set; } = new List<TranslationModel>();
        public virtual ICollection<SongModel> Songs { get; set; } = new List<SongModel>();
    }
}
