using Entities;
using System.Collections.Generic;

namespace Dtos
{
    internal class Song
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Lyrics { get; set; }
        public virtual List<WordModel> Words { get; set; } = new List<WordModel>();
    }
}
