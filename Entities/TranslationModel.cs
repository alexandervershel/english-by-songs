using System.Collections.Generic;

namespace Entities
{
    public class TranslationModel : IModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public virtual ICollection<WordModel> Words { get; set; } = new List<WordModel>();

        public override string ToString()
        {
            return Text;
        }
    }
}
