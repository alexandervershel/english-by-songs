namespace EnglishBySongs.ViewModels.ItemViewModels
{
    public interface IItemViewModel<TModel>
    {
        public bool IsSelected { get; set; }
        public string StringByWhichToFind { get; set; }
        TModel Model { get; }
    }
}
