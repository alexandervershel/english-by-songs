using Dal;
using Dal.Repositories;
using EnglishBySongs.Services;
using EnglishBySongs.Views;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace EnglishBySongs.ViewModels
{
    public class TrainingsMenuViewModel
    {
        private IPageService _pageService;
        private readonly WordRepository _wordRepository = new WordRepository(EnglishBySongsDbContext.GetInstance());
        public ICommand StartTrainingCommand { get; private set; }

        public TrainingsMenuViewModel()
        {
            _pageService = new PageService();
            StartTrainingCommand = new Command<string>(async (x) => await StartTraining(x));

        }

        private async Task StartTraining(string x)
        {
            bool trainingCannotBeStarted;
            trainingCannotBeStarted = _wordRepository.Get(w => !w.IsLearned) == null;
            if (trainingCannotBeStarted)
            {
                await _pageService.DisplayAlert("Недостаточно слов", "Для начала тренировки перейдите в раздел \"ДОБАВЛЕНИЕ\" и добавьте неизвестные вам слова", "ок");
                return;
            }

            switch (x)
            {
                case "0":
                    await _pageService.PushAsync(new CardsForeignNativeTrainingPage());
                    break;
                case "1":
                    await _pageService.PushAsync(new CardsNativeForeignTrainingPage());
                    break;
                default:
                    break;
            }
        }
    }
}
