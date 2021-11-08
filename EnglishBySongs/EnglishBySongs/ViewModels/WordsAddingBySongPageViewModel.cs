using EnglishBySongs.Data;
using EnglishBySongs.Models;
using EnglishBySongs.Services;
using EnglishBySongs.Views;
using Microsoft.EntityFrameworkCore;
using Plugin.Connectivity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EnglishBySongs.ViewModels
{
    public class WordsAddingBySongPageViewModel : BaseViewModel
    {
        public WordsAddingBySongPageViewModel()
        {
            _pageService = new PageService();

            SelectedWords = new List<object>();
            Words = new List<Word>();

            IsConnectedToInternet = CrossConnectivity.Current.IsConnected;
            CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;

            using (EnglishBySongsDbContext db = new EnglishBySongsDbContext())
            {
                AllWords = db.Words.ToList();
            }

            GetLyricsCommand = new Command(async () => await GetLyrics());
            ExtractWordsCommand = new Command(async () => await ExtractWords());
            AddSelectedWordsCommand = new Command(async () => await AddSelectedWords());
            HideAddedWordsCommand = new Command(() => SwapLists());
        }

        private IPageService _pageService;

        public ICommand GetLyricsCommand { get; private set; }

        public ICommand ExtractWordsCommand { get; private set; }

        public ICommand AddSelectedWordsCommand { get; private set; }

        public ICommand HideAddedWordsCommand { get; private set; }

        public List<Word> AllWords { get; set; }

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

        public bool LyricsAreNotFoundStackLayoutIsVisible
        {
            get { return _lyricsAreNotFoundStackLayoutIsVisible; }
            set
            {
                SetValue(ref _lyricsAreNotFoundStackLayoutIsVisible, value);
                OnPropertyChanged(nameof(LyricsAreNotFoundStackLayoutIsVisible));
            }
        }

        private bool _manuallyLyricsAddingStackLayoutIsVisible;

        private bool _songIsAlreadyAddedStachLayoutIsVisible;

        public bool SongIsAlreadyAddedStachLayoutIsVisible
        {
            get { return _songIsAlreadyAddedStachLayoutIsVisible; }
            set
            {
                SetValue(ref _songIsAlreadyAddedStachLayoutIsVisible, value);
                OnPropertyChanged(nameof(SongIsAlreadyAddedStachLayoutIsVisible));
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

        private string _performer;

        public string Performer
        {
            get { return _performer; }
            set
            {
                SetValue(ref _performer, value);
                OnPropertyChanged(nameof(Performer));
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
                return;

            Lyrics = string.Empty;
            FoundLyricsStackLayoutIsVisible = false;
            LyricsAreNotFoundStackLayoutIsVisible = false;
            SongIsAlreadyAddedStachLayoutIsVisible = false;

            using (EnglishBySongsDbContext db = new EnglishBySongsDbContext())
            {
                if (db.Songs.Any(s => s.Artist == Performer.Trim().ToLower() && s.Name == SongName.Trim().ToLower()))
                {
                    IsBusy = false;
                    SongIsAlreadyAddedStachLayoutIsVisible = true;
                    return;
                }
            }
            await _pageService.PushAsync(new LyricsPage(this));
            IsBusy = true;

            SongLyricsParser parser = new SongLyricsParser(_performer, _songName);

            string lyrics = await parser.ParseAsync();
            if (lyrics == null)
            {
                IsBusy = false;
                LyricsAreNotFoundStackLayoutIsVisible = true;
                return;
            }

            IsBusy = false;
            Lyrics = lyrics;

            FoundLyricsStackLayoutIsVisible = true;

        }

        private async Task ExtractWords()
        {
            Words = new List<Word>();
            WordsExtractor.Extract(Lyrics).ToList().ForEach(s => Words.Add(new Word() { Foreign = s }));

            NotAddedWords = new List<Word>(Words);
            using (EnglishBySongsDbContext db = new EnglishBySongsDbContext())
            {
                AllWords = db.Words.ToList();
            }

            NotAddedWords.RemoveAll(w => AllWords.Any(x => x.Foreign == w.Foreign));

            await _pageService.PushAsync(new WordsAddingPage(this));
        }

        private async Task AddSelectedWords()
        {
            IsBusy = true;
            using (var db = new EnglishBySongsDbContext())
            {

                Song song = new Song()
                {
                    Artist = _performer,
                    Name = _songName,
                    Lyrics = _lyrics
                };
                db.Songs.Add(song);

                bool isLearned;
                Word word;

                List<Word> words = _listsAreSwaped ? NotAddedWords : Words;
                bool autoTranslatingIsSwitchedOn = Preferences.Get("AutoTranslating", true);
                foreach (var w in words)
                {
                    isLearned = !SelectedWords.Any(p => ((Word)p).Foreign == w.Foreign);
                    if ((word = await db.Words.Include(w1 => w1.Translations).Include(w1 => w1.Songs).FirstOrDefaultAsync(w1 => w1.Foreign == w.Foreign)) == null)
                    {
                        word = new Word()
                        {
                            Foreign = w.Foreign,
                            IsLearned = isLearned
                        };
                        db.Words.Add(word);
                    }
                    else
                    {
                        if (word.IsLearned)
                            word.IsLearned = isLearned;
                    }

                    // TODO: выделенные слова, при их присутствии в выученных словах, переносить в невыученыне
                    if (!isLearned && word.Translations.Count == 0 && autoTranslatingIsSwitchedOn)
                    {
                        WordsTranslationsParser wordsTranslationsParser = new WordsTranslationsParser();
                        List<Translation> translations;
                        // TODO: убрать промежуточную коллекцию
                        translations = new List<Translation>();
                        List<string> tr = await wordsTranslationsParser.TranslateAsync(w.Foreign);
                        if (tr != null)
                        {
                            tr.ForEach(t => translations.Add(new Translation { Text = t }));
                            foreach (var t in translations)
                            {
                                if (await db.Translations.FirstOrDefaultAsync(t1 => t1.Text == t.Text) == null)
                                {
                                    db.Translations.Add(t);
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
                db.SaveChanges();
            }

            Performer = string.Empty;
            SongName = string.Empty;
            FoundLyricsStackLayoutIsVisible = false;

            MessagingCenter.Send(this, "WordsAdded");
            MessagingCenter.Send(this, "SongAdded");
            IsBusy = false;

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
