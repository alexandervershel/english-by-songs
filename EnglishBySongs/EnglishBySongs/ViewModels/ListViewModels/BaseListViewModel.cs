using Core.Interfaces;
using EnglishBySongs.ViewModels.ItemViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace EnglishBySongs.ViewModels.ListViewModels
{
    public abstract class BaseListViewModel<TItem, TModel> : PageServiceViewModel where TItem : IItemViewModel<TModel> where TModel : IModel
    {
        private ObservableCollection<TItem> _items;
        private ObservableCollection<TItem> _allItems;
        private TItem _selectedItem;
        private string _itemsSearchQuery;
        private bool _isMultiselect;
        public virtual ICommand ItemTappedCommand { get; private set; }
        public virtual ICommand DisplayCheckBoxesCommand { get; private set; }
        public virtual ICommand CancelMultiselectCommand { get; private set; }
        public virtual ICommand DeleteWordsCommand { get; private set; }
        public virtual ICommand SelectAllCommand { get; private set; }
        public BaseListViewModel() : base()
        {
            _isMultiselect = false;
            Items = new ObservableCollection<TItem>();
            ReadCollectionFromDb();
            Sort();
            AllItems = Items;

            DisplayCheckBoxesCommand = new Command(async () => await EnableMultiselect());
            CancelMultiselectCommand = new Command(async () => await DisableMultiselect());
            ItemTappedCommand = new Command<TItem>(async (item) => await ItemTapped(item));
            DeleteWordsCommand = new Command<object>(async (obj) => await DeleteItems(obj));
            SelectAllCommand = new Command(async () => await SelectAll());
            
        }

        public ObservableCollection<TItem> Items
        {
            get { return _items; }
            set
            {
                SetValue(ref _items, value);
                OnPropertyChanged(nameof(Items));
            }
        }

        public ObservableCollection<TItem> AllItems
        {
            get { return _allItems; }
            set
            {
                SetValue(ref _allItems, value);
                OnPropertyChanged(nameof(AllItems));
            }
        }

        public ObservableCollection<TItem> SelectedItems
        {
            get
            {
                return new ObservableCollection<TItem>(AllItems.Where(w => w.IsSelected).ToList());
            }
        }

        public TItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                SetValue(ref _selectedItem, value);
                OnPropertyChanged(nameof(SelectedItem));

                //SelectedItem.ToEditPage();
                ToItemEditPage();
            }
        }

        public string ItemsSearchQuery
        {
            get { return _itemsSearchQuery; }
            set
            {
                SetValue(ref _itemsSearchQuery, value);
                OnPropertyChanged(nameof(ItemsSearchQuery));

                Items = new ObservableCollection<TItem>(AllItems.Where(i => i.StringByWhichToFind.Contains(_itemsSearchQuery)).ToList());
            }
        }

        public bool IsMultiselect
        {
            get { return _isMultiselect; }
            set
            {
                SetValue(ref _isMultiselect, value);
                OnPropertyChanged(nameof(IsMultiselect));
            }
        }

        protected abstract void ReadCollectionFromDb();

        protected abstract Task Sort();

        protected abstract Task DeleteItems(object obj);

        protected abstract Task ToItemEditPage();

        protected async Task RefreshAsync()
        {
            ReadCollectionFromDb();
            Sort();
            AllItems = Items;
        }

        protected async Task ItemTapped(TItem item)
        {
            if (!IsMultiselect)
            {
                SelectedItem = item;
            }
            else
            {
                item.IsSelected = !item.IsSelected;
            }
        }

        protected async Task EnableMultiselect()
        {
            IsMultiselect = true;
        }

        protected async Task SelectAll()
        {
            AllItems.ForEach(w => w.IsSelected = true);
        }

        protected async Task DisableMultiselect()
        {
            IsMultiselect = false;
            AllItems.ForEach(w => w.IsSelected = false);
        }
    }
}
