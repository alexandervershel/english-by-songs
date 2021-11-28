using System.Threading.Tasks;

namespace ViewModels.Dtos
{
    public interface IListViewItemViewModel
    {
        public Task ToEditPage();
        public bool IsSelected { get; set; }
        public string StringByWhichToFind { get; set; }
    }
}
