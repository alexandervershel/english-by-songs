using EnglishBySongs.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace EnglishBySongs.ViewModels
{
    public class TrainingsMenuViewModel
    {
        private IPageService _pageService;
        public ICommand StartTrainingCommand { get; private set; }

        public TrainingsMenuViewModel()
        {
            _pageService = new PageService();
            StartTrainingCommand = new Command<string>(async (x) => await StartTraining(x));

        }

        private async Task StartTraining(string x)
        {
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
