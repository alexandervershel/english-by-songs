using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Entities
{
    public class SongModel : IModel
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Artist { get; set; }

        public string Lyrics { get; set; }

        public virtual List<WordModel> Words { get; set; } = new List<WordModel>();

        // TODO: по возможности удалить это свойство из модели
        [NotMapped]
        public ICollection<WordModel> UnlearnedWords
        {
            get
            {
                return Words.Where(w => !w.IsLearned).ToList();
            }
        }

        // TODO: это свойство тоже надо удалить
        [NotMapped]
        public ICollection<WordModel> LearnedWords
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
