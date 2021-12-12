using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Entities
{
    public class Word : IModel
    {
        public int Id { get; set; }
        public string Foreign { get; set; }
        public bool IsLearned { get; set; }
        public virtual ICollection<Translation> Translations { get; set; } = new List<Translation>();
        public virtual ICollection<Song> Songs { get; set; } = new List<Song>();

        [NotMapped]
        public string Line
        {
            get
            {
                List<Song> songs = Songs.ToList();
                return songs[0].Lyrics.Split("\n").FirstOrDefault(s => s.Contains(Foreign, StringComparison.OrdinalIgnoreCase));
            }
        }

    }
}
