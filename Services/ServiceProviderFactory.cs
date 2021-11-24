using Dal;
using Entities;
using Microsoft.Extensions.DependencyInjection;
using Services.Interfaces;
using Services.Parsers;
using Services.Repositories;
using Services.UserInteraction;
using System;

namespace Services
{
    public static class ServiceProviderFactory
    {
        public static IServiceProvider ServiceProvider { get; }

        static ServiceProviderFactory()
        {
            var services = new ServiceCollection()
                .AddSingleton<IRepository<Word>>(new WordRepository())
                .AddSingleton<IRepository<Song>>(new SongRepository())
                .AddSingleton<IRepository<Translation>>(new TranslationRepository())
                .AddSingleton<IPageService>(new PageService())
                .AddSingleton<ISongLyricsParser>(new SongLyricsParser())
                .AddSingleton<IWordsTranslationsParser>(new WordsTranslationsParser());
            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
