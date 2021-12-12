using Core.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Entities
{
    public class Song : IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Lyrics { get; set; }
        public List<Word> Words { get; set; } = new List<Word>();
        [NotMapped]
        public string FullName { get => $"{Artist} - {Name}"; }
        // TODO: по возможности удалить это свойство из модели
        [NotMapped]
        public ICollection<Word> UnlearnedWords => Words.Where(w => !w.IsLearned).ToList();

        // TODO: это свойство тоже надо удалить
        [NotMapped]
        public ICollection<Word> LearnedWords => Words.Where(w => w.IsLearned).ToList();

        // TODO: make it a property
        public override string ToString()
        {
            return $"{Artist} - {Name}";
        }
    }
}
