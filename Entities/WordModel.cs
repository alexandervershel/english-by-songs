using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Entities
{
    public class WordModel : IModel
    {
        public int Id { get; set; }
        public string Foreign { get; set; }
        public bool IsLearned { get; set; }
        public virtual ICollection<TranslationModel> Translations { get; set; } = new List<TranslationModel>();
        public virtual ICollection<SongModel> Songs { get; set; } = new List<SongModel>();
        [NotMapped]
        public string Line
        {
            get
            {
                List<SongModel> songs = Songs.ToList();
                return songs[0].Lyrics.Split("\n").FirstOrDefault(s => s.Contains(Foreign, StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}
