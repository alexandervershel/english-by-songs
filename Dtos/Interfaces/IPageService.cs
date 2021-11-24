using EnglishBySongs.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Dtos.Interfaces
{
    public interface IPageService
    {
        Task PushAsync(Page page);
        Task<Page> PopAsync();
        Task<bool> DisplayAlert(string title, string message, string ok, string cancel);
        Task DisplayAlert(string title, string message, string ok);
        Task DispayToast(string message);
        Task PushEditPageAsync<T>() where T : IListViewItemViewModel;
    }
}
