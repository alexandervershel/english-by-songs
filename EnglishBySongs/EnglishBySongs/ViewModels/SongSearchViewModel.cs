using Core.Helpers;
using Dal;
using Dal.Repositories;
using EnglishBySongs.Services;
using EnglishBySongs.Views;
using Entities;
using Plugin.Connectivity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EnglishBySongs.ViewModels
{
    // TODO: name as SongSearchViewModel
    public class SongSearchViewModel : BaseViewModel
    {
        public ICommand GetLyricsCommand { get; private set; }
        public ICommand ExtractWordsCommand { get; private set; }
        public ICommand AddSelectedWordsCommand { get; private set; }
        public ICommand HideAddedWordsCommand { get; private set; }
        public List<Word> AllWords { get; set; }
        private readonly WordRepository _wordRepository = new WordRepository(EnglishBySongsDbContext.GetInstance());
        private readonly SongRepository _songRepository = new SongRepository(EnglishBySongsDbContext.GetInstance());
        private readonly WordsTranslationsParser _wordsTranslationsParser = new WordsTranslationsParser();
        private readonly SongLyricsParser _songLyricsParser = new SongLyricsParser();
        private readonly TranslationRepository _translationRepository = new TranslationRepository(EnglishBySongsDbContext.GetInstance());
        private IPageService _pageService;
        public SongSearchViewModel()
        {
            _pageService = new PageService();
            SelectedWords = new List<object>();
            Words = new List<Word>();
            IsConnectedToInternet = CrossConnectivity.Current.IsConnected;
            CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;
            AllWords = _wordRepository.GetAll();

            GetLyricsCommand = new Command(async () => await GetLyrics());
            ExtractWordsCommand = new Command(async () => await ExtractWords());
            AddSelectedWordsCommand = new Command(async () => await AddSelectedWords());
            HideAddedWordsCommand = new Command(() => SwapLists());
        }

        private bool _foundLyricsStackLayoutIsVisible;

        public bool FoundLyricsStackLayoutIsVisible
        {
            get { return _foundLyricsStackLayoutIsVisible; }
            set
            {
                SetValue(ref _foundLyricsStackLayoutIsVisible, value);
                OnPropertyChanged(nameof(FoundLyricsStackLayoutIsVisible));
            }
        }

        private bool _lyricsAreNotFoundStackLayoutIsVisible;

        public bool LyricsAreNotFound
        {
            get { return _lyricsAreNotFoundStackLayoutIsVisible; }
            set
            {
                SetValue(ref _lyricsAreNotFoundStackLayoutIsVisible, value);
                OnPropertyChanged(nameof(LyricsAreNotFound));
            }
        }

        private bool _songIsAlreadyAddedStachLayoutIsVisible;

        public bool SameSongExists
        {
            get { return _songIsAlreadyAddedStachLayoutIsVisible; }
            set
            {
                SetValue(ref _songIsAlreadyAddedStachLayoutIsVisible, value);
                OnPropertyChanged(nameof(SameSongExists));
            }
        }

        private bool _addedWordsAreHidden;

        public bool AddedWordsAreHidden
        {
            get { return _addedWordsAreHidden; }
            set
            {
                SetValue(ref _addedWordsAreHidden, value);
                OnPropertyChanged(nameof(AddedWordsAreHidden));
                SwapLists();
            }
        }

        private string _lyrics;

        public string Lyrics
        {
            get { return _lyrics; }
            set
            {
                SetValue(ref _lyrics, value);
                OnPropertyChanged(nameof(Lyrics));
            }
        }

        private string _artist;

        public string Artist
        {
            get { return _artist; }
            set
            {
                SetValue(ref _artist, value);
                OnPropertyChanged(nameof(Artist));
            }
        }

        private string _songName;

        public string SongName
        {
            get { return _songName; }
            set
            {
                SetValue(ref _songName, value);
                OnPropertyChanged(nameof(SongName));
            }
        }

        private List<Word> _words;

        public List<Word> Words
        {
            get { return _words; }
            set
            {
                SetValue(ref _words, value);
                OnPropertyChanged(nameof(Words));
            }
        }

        private List<Word> _notAddedWords;

        public List<Word> NotAddedWords
        {
            get { return _notAddedWords; }
            set
            {
                SetValue(ref _notAddedWords, value);
                OnPropertyChanged(nameof(NotAddedWords));
            }
        }

        private List<object> _selectedWords;

        public List<object> SelectedWords
        {
            get { return _selectedWords; }
            set
            {
                SetValue(ref _selectedWords, value);
                OnPropertyChanged(nameof(SelectedWords));
            }
        }

        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                SetValue(ref _isLoading, value);
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        private bool _isConnectedToInternet;

        public bool IsConnectedToInternet
        {
            get { return CrossConnectivity.Current.IsConnected; }
            set
            {
                SetValue(ref _isConnectedToInternet, value);
                OnPropertyChanged(nameof(IsConnectedToInternet));
            }
        }

        private async Task GetLyrics()
        {
            // TODO: исправить отслеживание подключения к интернету
            if (!IsConnectedToInternet)
            {
                return;
            }

            Lyrics = string.Empty;
            FoundLyricsStackLayoutIsVisible = false;
            LyricsAreNotFound = false;
            SameSongExists = false;
            if (_songRepository.Get(s => s.Artist == Artist.Trim().ToLower() && s.Name == SongName.Trim().ToLower()) != null)
            {
                IsLoading = false;
                SameSongExists = true;
                return;
            }

            await _pageService.PushAsync(new LyricsPage(this));
            IsLoading = true;
            string lyrics = await _songLyricsParser.ParseAsync(_artist, _songName);
            if (lyrics == null)
            {
                IsLoading = false;
                LyricsAreNotFound = true;
                return;
            }

            IsLoading = false;
            Lyrics = lyrics;

            FoundLyricsStackLayoutIsVisible = true;

        }

        private async Task ExtractWords()
        {
            Words = new List<Word>();
            WordsExtractor.Extract(Lyrics).ToList().ForEach(s => Words.Add(new Word() { Foreign = s }));
            NotAddedWords = new List<Word>(Words);
            AllWords = _wordRepository.GetAll();
            NotAddedWords.RemoveAll(w => AllWords.Any(x => x.Foreign == w.Foreign));
            await _pageService.PushAsync(new WordsAddingPage(this));
        }

        private async Task AddSelectedWords()
        {
            IsLoading = true;
            Song song = new Song()
            {
                Artist = _artist,
                Name = _songName,
                Lyrics = _lyrics
            };
            //_songRepository.Add(song);

            //bool isLearned;
            //Word word;
            //List<Word> words = _listsAreSwaped ? NotAddedWords : Words;
            //bool autoTranslatingIsSwitchedOn = Preferences.Get("AutoTranslating", true);
            //foreach (var w in words)
            //{
            //    isLearned = !SelectedWords.Any(p => ((Word)p).Foreign == w.Foreign);
            //    if ((word = _wordRepository.Get(w1 => w1.Foreign == w.Foreign)) == null)
            //    {
            //        word = new Word()
            //        {
            //            Foreign = w.Foreign,
            //            IsLearned = isLearned
            //        };
            //        _wordRepository.Add(word);
            //    }
            //    else
            //    {
            //        if (word.IsLearned)
            //        {
            //            word.IsLearned = isLearned;
            //        }
            //    }

            //    // TODO: выделенные слова, при их присутствии в выученных словах, переносить в невыученыне
            //    if (!isLearned && word.Translations.Count == 0 && autoTranslatingIsSwitchedOn)
            //    {
            //        List<Translation> translations;
            //        // TODO: убрать промежуточную коллекцию
            //        translations = new List<Translation>();
            //        List<string> tr = await _wordsTranslationsParser.TranslateAsync(w.Foreign);
            //        if (tr != null)
            //        {
            //            tr.ForEach(t => translations.Add(new Translation { Text = t }));
            //            foreach (var t in translations)
            //            {
            //                if (_translationRepository.Get(t1 => t1.Text == t.Text) == null)
            //                {
            //                    _translationRepository.Add(t);
            //                }
            //            }
            //        }
            //        if (translations.Count != 0)
            //        {
            //            translations.ForEach(t => word.Translations.Add(t));
            //        }
            //    }
            //    word.Songs.Add(song);
            //}
            //_wordRepository.Save();
            await DbHelper.AddSongWithWords(song, _listsAreSwaped ? NotAddedWords : Words, SelectedWords, Preferences.Get("AutoTranslating", true));
            Artist = string.Empty;
            SongName = string.Empty;
            FoundLyricsStackLayoutIsVisible = false;

            MessagingCenter.Send(this, "WordsAdded");
            MessagingCenter.Send(this, "SongAdded");
            IsLoading = false;

            _listsAreSwaped = false;
            _addedWordsAreHidden = false;

            await _pageService.PopAsync();
            await _pageService.PopAsync();
            await _pageService.DispayToast("Слова добавлены в словарь");
        }

        private bool _listsAreSwaped = false;
        private void SwapLists()
        {
            if (AddedWordsAreHidden == _listsAreSwaped && AddedWordsAreHidden)
            {
                _addedWordsAreHidden = !_addedWordsAreHidden;
                return;
            }

            var temporary = Words;
            Words = NotAddedWords;
            NotAddedWords = temporary;

            _listsAreSwaped = !_listsAreSwaped;
        }

        private void Current_ConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            IsConnectedToInternet = e.IsConnected;
        }
    }
}
