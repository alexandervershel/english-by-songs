using EnglishBySongs.Data;
using EnglishBySongs.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace EnglishBySongs.ViewModels
{
    public class WordViewModel : BaseViewModel
    {
        private IPageService _pageService;

        public ICommand SaveChangesCommand { get; private set; }

        public ICommand AddNewTranslationCommand { get; private set; }

        public ICommand RemoveTranslationCommand { get; private set; }

        public WordViewModel()
        {
            SaveChangesCommand = new Command(async () => await SaveChanges());
            AddNewTranslationCommand = new Command(async () => await AddNewTranslation());
            RemoveTranslationCommand = new Command(async (translation) => await RemoveTranslation(translation));

            _pageService = new PageService();
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

        private Word _primaryWord;

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
            using (EnglishBySongsDbContext db = new EnglishBySongsDbContext())
            {
                Translation translation;
                List<Translation> translations = new List<Translation>();
                foreach (var tr in Translations.ToList())
                {
                    if ((translation = await db.Translations.FirstOrDefaultAsync(t => t.Text == tr.Text)) == null)
                    {
                        if (!string.IsNullOrWhiteSpace(tr.Text))
                        {
                            translations.Add(tr);
                            db.Translations.Add(tr);
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
                db.SaveChanges();

                Word word = db.Words.Include(w => w.Translations).FirstOrDefault(w => w.Id == _primaryWord.Id);

                word.Translations = translations;
                word.IsLearned = IsLearned;
                db.SaveChanges();
            }

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
    }
}
