using System.Threading.Tasks;

namespace EnglishBySongs.ViewModels
{
    public interface IListViewItemViewModel
    {
        public Task ToEditPage();
        public bool IsSelected { get; set; }
        public string StringByWhichToFind { get; set; }
    }
}
