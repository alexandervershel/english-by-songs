using Entities;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Interfaces;
using Services.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModels.Helpers
{
    internal static class DbHelper
    {
        private static readonly IServiceProvider _serviceProvider = ServiceProviderFactory.ServiceProvider;
        private static readonly IWordsTranslationsParser _wordsTranslationsParser = _serviceProvider.GetService<IWordsTranslationsParser>();
        private static readonly IRepository<Translation> _translationRepository = _serviceProvider.GetService<IRepository<Translation>>();
        private static readonly IRepository<Song> _songRepository = _serviceProvider.GetService<IRepository<Song>>();
        private static readonly IRepository<Word> _wordRepository = _serviceProvider.GetService<IRepository<Word>>();
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
