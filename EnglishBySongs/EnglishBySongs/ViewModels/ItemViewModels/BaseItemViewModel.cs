using Core.Interfaces;

namespace EnglishBySongs.ViewModels.ItemViewModels
{
    public class BaseItemViewModel<TModel> : BaseViewModel, IItemViewModel<TModel> where TModel : IModel
    {
        private bool _isSelected;
        public string StringByWhichToFind { get; set; }
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                SetValue(ref _isSelected, value);
                OnPropertyChanged(nameof(IsSelected));
            }
        }
        public TModel Model { get; private set; }
        public BaseItemViewModel(TModel model)
        {
            Model = model;
        }
    }
}
