using EnglishBySongs.Models;
using EnglishBySongs.Services;
using EnglishBySongs.Views;
using System.Threading.Tasks;

namespace EnglishBySongs.ViewModels
{
    public class WordItem : Word, ISelectable, IEditable, ISearchable
    {
        private IPageService _pageService;

        //public WordItem()
        //{
        //    _pageService = new PageService();
        //    stringByWhichToFind = Foreign;
        //}
        public WordItem()
        {

        }

        public WordItem(Word word)// : this()
        {
            _pageService = new PageService();

            Id = word.Id;
            Foreign = word.Foreign;
            IsLearned = word.IsLearned;
            Translations = word.Translations;
            Songs = word.Songs;

            stringByWhichToFind = Foreign;
        }

        // TODO: перенести в базовый класс
        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                SetValue(ref _isSelected, value);
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public string stringByWhichToFind { get; set; }

        public async Task ToEditPage()
        {
            await _pageService.PushAsync(new WordPage(new WordViewModel(this)));
        }
    }
}
