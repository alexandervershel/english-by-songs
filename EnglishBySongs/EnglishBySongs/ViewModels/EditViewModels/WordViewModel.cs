using Entities;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Interfaces;
using Services.Parsers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace EnglishBySongs.ViewModels.EditViewModels
{
    public class WordViewModel : BaseViewModel
    {
        private static readonly IServiceProvider _serviceProvider = ServiceProviderFactory.ServiceProvider;
        private readonly IRepository<Word> _wordRepository;
        private readonly IRepository<Translation> _translationRepository;
        private readonly IPageService _pageService;
        private readonly IWordsTranslationsParser _translationsParser;
        public ICommand SaveChangesCommand { get; private set; }
        public ICommand AddNewTranslationCommand { get; private set; }
        public ICommand RemoveTranslationCommand { get; private set; }
        public ICommand TranslateWordCommand { get; private set; }
        private readonly Word _primaryWord;
        public WordViewModel()
        {
            _wordRepository = _serviceProvider.GetService<IRepository<Word>>();
            _translationRepository = _serviceProvider.GetService<IRepository<Translation>>();
            _translationsParser = _serviceProvider.GetService<IWordsTranslationsParser>();
            _pageService = _serviceProvider.GetService<IPageService>();
            SaveChangesCommand = new Command(async () => await SaveChanges());
            AddNewTranslationCommand = new Command(async () => await AddNewTranslation());
            RemoveTranslationCommand = new Command(async (translation) => await RemoveTranslation(translation));
            TranslateWordCommand = new Command(async () => await TranslateWord());
        }

        public WordViewModel(Word word) : this()
        {
            _primaryWord = word;
            Foreign = word.Foreign;
            IsLearned = word.IsLearned;
            Translations = new ObservableCollection<Translation>(word.Translations.ToList());
            if (Translations.Count == 1 && string.IsNullOrWhiteSpace(Translations[0].Text))
            {
                Translations = new ObservableCollection<Translation>();
            }
            Songs = word.Songs.ToList();
        }
        public Word ToWord()
        {
            return new Word()
            {
                Foreign = Foreign,
                IsLearned = IsLearned,
                Translations = Translations,
                Songs = Songs
            };
        }

        private string _foreign;

        public string Foreign
        {
            get { return _foreign; }
            set
            {
                SetValue(ref _foreign, value);
                OnPropertyChanged(nameof(Foreign));
            }
        }

        private bool _isLearned;

        public bool IsLearned
        {
            get { return _isLearned; }
            set
            {
                SetValue(ref _isLearned, value);
                OnPropertyChanged(nameof(IsLearned));
            }
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                SetValue(ref _isBusy, value);
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        private ObservableCollection<Translation> _translations;

        public ObservableCollection<Translation> Translations
        {
            get { return _translations; }
            set
            {
                SetValue(ref _translations, value);
                OnPropertyChanged(nameof(Translations));
            }
        }

        private List<Song> _songs;

        public List<Song> Songs
        {
            get { return _songs; }
            set
            {
                SetValue(ref _songs, value);
                OnPropertyChanged(nameof(Songs));
            }
        }

        private async Task SaveChanges()
        {
            Translation translation;
            List<Translation> translations = new List<Translation>();
            foreach (var tr in Translations.ToList())
            {
                if ((translation = _translationRepository.Get(t => t.Text == tr.Text)) == null)
                {
                    if (!string.IsNullOrWhiteSpace(tr.Text))
                    {
                        translations.Add(tr);
                        _translationRepository.Add(tr);
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(translation.Text))
                    {
                        translations.Add(translation);
                    }
                }
            }
            _wordRepository.Save();
            Word word = _wordRepository.Get(w => w.Id == _primaryWord.Id);
            word.Translations = translations;
            word.IsLearned = IsLearned;
            _wordRepository.Save();

            MessagingCenter.Send(this, "WordUpdated");
            await _pageService.PopAsync();
            await _pageService.DispayToast("Изменения сохранены");
        }

        private async Task AddNewTranslation()
        {
            Translations.Add(new Translation() { Text = string.Empty });
            OnPropertyChanged(nameof(Translations));
        }

        private async Task RemoveTranslation(object translation)
        {
            Translations.Remove((Translation)translation);
        }

        private async Task TranslateWord()
        {
            IsBusy = true;
            Translations.Clear();
            (await _translationsParser.TranslateAsync(_primaryWord.Foreign)).ForEach(t => Translations.Add(new Translation() { Text = t }));
            Translations = Translations;
            IsBusy = false;
        }
    }
}
