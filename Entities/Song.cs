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

        public virtual List<Word> Words { get; set; } = new List<Word>();

        // TODO: по возможности удалить это свойство из модели
        [NotMapped]
        public ICollection<Word> UnlearnedWords
        {
            get
            {
                return Words.Where(w => !w.IsLearned).ToList();
            }
        }

        // TODO: это свойство тоже надо удалить
        [NotMapped]
        public ICollection<Word> LearnedWords
        {
            get
            {
                return Words.Where(w => w.IsLearned).ToList();
            }
        }

        public override string ToString()
        {
            return $"{Artist} - {Name}";
        }
    }
}
