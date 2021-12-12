using EnglishBySongs.Interfaces;
using EnglishBySongs.ViewModels;
using Services.UserInteraction;

namespace EnglishBySongs
{
    public class PageServiceViewModel : BaseViewModel
    {
        protected readonly IPageService _pageService;
        public PageServiceViewModel()
        {
            _pageService = new PageService();
        }
    }
}
