using System.Collections.Generic;

namespace Entities
{
    public class Translation : IModel
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
