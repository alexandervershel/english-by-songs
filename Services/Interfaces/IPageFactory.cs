using Xamarin.Forms;

namespace Services.Interfaces
{
    internal interface IPageFactory
    {
        Page GetWordPage();
        Page GetSongPage();
    }
}
