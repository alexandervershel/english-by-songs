using System.Collections.Generic;

namespace EnglishBySongs.Models
{
    public class Translation
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public virtual ICollection<Word> Words { get; set; } = new List<Word>();

        public override string ToString()
        {
            return Text;
        }
    }
}
