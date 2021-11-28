using EnglishBySongs.Views;
using Entities;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Interfaces;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ViewModels
{
    public class TrainingsMenuViewModel : BaseViewModel
    {
        private static readonly IServiceProvider _serviceProvider = ServiceProviderFactory.ServiceProvider;
        private readonly IPageService _pageService;
        private readonly IRepository<Word> _wordRepository;
        public ICommand StartTrainingCommand { get; private set; }

        public TrainingsMenuViewModel()
        {
            _pageService = _serviceProvider.GetService<IPageService>();
            _wordRepository = _serviceProvider.GetService<IRepository<Word>>();
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
