using EnglishBySongs.Data;
using EnglishBySongs.Models;
using EnglishBySongs.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace EnglishBySongs.ViewModels
{
    class TrainingViewModel : BaseViewModel
    {
        private IPageService _pageService;

        public ICommand PopPageCommand { get; private set; }

        public ICommand EndTrainingCommand { get; private set; }

        public TrainingViewModel()
        {
            _pageService = new PageService();

            PopPageCommand = new Command(async () => await PopPage());
            EndTrainingCommand = new Command(async () => await EndTraining());

            Random rng = new Random();
            using (EnglishBySongsDbContext db = new EnglishBySongsDbContext())
            {
                _words = db.Words.Include(w => w.Translations).Include(w => w.Songs).Where(w => !w.IsLearned).ToList().OrderBy(a => rng.Next()).ToList();
            }
        }

        private async Task PopPage()
        {
            await EndTraining();
        }

        private async Task EndTraining()
        {
            List<Word> changedWords = Words.Where(w=>w.IsLearned).ToList();
            using (EnglishBySongsDbContext db = new EnglishBySongsDbContext())
            {
                foreach (Word word in changedWords)
                {
                    (await db.Words.FirstOrDefaultAsync(w => w.Foreign == word.Foreign)).IsLearned = true;
                }
                db.SaveChanges();
            }
            MessagingCenter.Send(new WordViewModel(), "WordUpdated");
            await _pageService.PopAsync();
            await _pageService.DispayToast("Тренировка закончена");
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

        private Word _currentWord;

        public Word CurrentWord
        {
            get { return _currentWord; }
            set
            {
                SetValue(ref _currentWord, value);
                OnPropertyChanged(nameof(CurrentWord));
                Words.FirstOrDefault(w => w.Foreign == _currentWord.Foreign).IsLearned = _currentWord.IsLearned;
            }
        }



    }
}
