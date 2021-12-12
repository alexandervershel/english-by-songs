using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Entities;
using Services.Interfaces;
using Services;
using Microsoft.Extensions.DependencyInjection;
using EnglishBySongs.ViewModels.EditViewModels;

namespace EnglishBySongs.ViewModels
{
    class TrainingViewModel : PageServiceViewModel
    {
        private static readonly IServiceProvider _serviceProvider = ServiceProviderFactory.ServiceProvider;
        private readonly IRepository<Word> _wordRepository;
        public ICommand PopPageCommand { get; private set; }
        public ICommand EndTrainingCommand { get; private set; }
        public TrainingViewModel() : base()
        {
            _wordRepository = _serviceProvider.GetService<IRepository<Word>>();

            PopPageCommand = new Command(async () => await PopPage());
            EndTrainingCommand = new Command(async () => await EndTraining());

            Random rng = new Random();
            _words = _wordRepository.GetAll(w => !w.IsLearned).OrderBy(a => rng.Next()).ToList();
        }

        private async Task PopPage()
        {
            await EndTraining();
        }

        private async Task EndTraining()
        {
            List<Word> changedWords = Words.Where(w=>w.IsLearned).ToList();
            foreach (Word word in changedWords)
            {
                _wordRepository.Get(w => w.Foreign == word.Foreign).IsLearned = true;
            }
            _wordRepository.Save();
            MessagingCenter.Send(new WordEditViewModel(), "WordUpdated");
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
