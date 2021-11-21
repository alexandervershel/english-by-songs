using Dal;
using Dal.Repositories;
using Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Helpers
{
    public static class DbHelper
    {
        private static readonly WordsTranslationsParser _wordsTranslationsParser = new WordsTranslationsParser();
        private static readonly SongLyricsParser _songLyricsParser = new SongLyricsParser();
        private static readonly TranslationRepository _translationRepository = new TranslationRepository(EnglishBySongsDbContext.GetInstance());
        private static readonly SongRepository _songRepository = new SongRepository(EnglishBySongsDbContext.GetInstance());
        private static readonly WordRepository _wordRepository = new WordRepository(EnglishBySongsDbContext.GetInstance());

        public static async Task AddSongWithWords(Song song, List<Word> words, List<object> selectedWords, bool autoTranslating)
        {
            _songRepository.Add(song);

            bool isLearned;
            Word word;
            foreach (var w in words)
            {
                isLearned = !selectedWords.Any(p => ((Word)p).Foreign == w.Foreign);
                if ((word = _wordRepository.Get(w1 => w1.Foreign == w.Foreign)) == null)
                {
                    word = new Word()
                    {
                        Foreign = w.Foreign,
                        IsLearned = isLearned
                    };
                    _wordRepository.Add(word);
                }
                else
                {
                    if (word.IsLearned)
                    {
                        word.IsLearned = isLearned;
                    }
                }

                // TODO: выделенные слова, при их присутствии в выученных словах, переносить в невыученыне
                if (!isLearned && word.Translations.Count == 0 && autoTranslating)
                {
                    List<Translation> translations;
                    // TODO: убрать промежуточную коллекцию
                    translations = new List<Translation>();
                    List<string> tr = await _wordsTranslationsParser.TranslateAsync(w.Foreign);
                    if (tr != null)
                    {
                        tr.ForEach(t => translations.Add(new Translation { Text = t }));
                        foreach (var t in translations)
                        {
                            if (_translationRepository.Get(t1 => t1.Text == t.Text) == null)
                            {
                                _translationRepository.Add(t);
                            }
                        }
                    }
                    if (translations.Count != 0)
                    {
                        translations.ForEach(t => word.Translations.Add(t));
                    }
                }
                word.Songs.Add(song);
            }
            _wordRepository.Save();
        }
    }
}
