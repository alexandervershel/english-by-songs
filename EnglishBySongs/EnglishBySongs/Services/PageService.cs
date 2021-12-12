using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Extensions;
using EnglishBySongs.Interfaces;

namespace Services.UserInteraction
{
    public class PageService : IPageService
    {
        private readonly Page _page = Application.Current.MainPage;

        public async Task DisplayAlert(string title, string message, string ok)
        {
            await _page.DisplayAlert(title, message, ok);
        }

        public async Task<bool> DisplayAlert(string title, string message, string ok, string cancel)
        {
            return await _page.DisplayAlert(title, message, ok, cancel);
        }

        public async Task PushAsync(Page page)
        {
            await _page.Navigation.PushAsync(page);
        }

        public async Task<Page> PopAsync()
        {
            return await _page.Navigation.PopAsync();
        }

        public async Task DispayToast(string message)
        {
            await _page.DisplayToastAsync(message, 2000);
        }
    }
}
