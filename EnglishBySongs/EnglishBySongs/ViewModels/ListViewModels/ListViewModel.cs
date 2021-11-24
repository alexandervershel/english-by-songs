using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace EnglishBySongs.ViewModels.ListViewModels
{
    // TODO: rename to 'MultiselectListViewModel'
    public abstract class ListViewModel<T> : BaseViewModel where T : IListViewItemViewModel
    {
        private static readonly IServiceProvider _serviceProvider = ServiceProviderFactory.ServiceProvider;
        protected readonly IPageService _pageService;
        public virtual ICommand ItemTappedCommand { get; private set; }
        public virtual ICommand DisplayCheckBoxesCommand { get; private set; }
        public virtual ICommand CancelMultiselectCommand { get; private set; }
        public virtual ICommand DeleteWordsCommand { get; private set; }
        public virtual ICommand SelectAllCommand { get; private set; }
        public ListViewModel()
        {
            _pageService = _serviceProvider.GetService<IPageService>();
            _isMultiselect = false;
            Items = new ObservableCollection<T>();
            ReadCollectionFromDb();
            Sort();
            AllItems = Items;

            DisplayCheckBoxesCommand = new Command(async () => await DisplayCheckBoxes());
            CancelMultiselectCommand = new Command(async () => await CancelMultiselect());
            ItemTappedCommand = new Command<T>(async (item) => await ItemTapped(item));
            DeleteWordsCommand = new Command<object>(async (obj) => await DeleteItems(obj));
            SelectAllCommand = new Command(async () => await SelectAll());
        }

        private ObservableCollection<T> _items;
        public ObservableCollection<T> Items
        {
            get { return _items; }
            set
            {
                SetValue(ref _items, value);
                OnPropertyChanged(nameof(Items));
            }
        }

        private ObservableCollection<T> _allItems;
        public ObservableCollection<T> AllItems
        {
            get { return _allItems; }
            set
            {
                SetValue(ref _allItems, value);
                OnPropertyChanged(nameof(AllItems));
            }
        }

        public ObservableCollection<T> SelectedItems
        {
            get
            {
                return new ObservableCollection<T>(AllItems.Where(w => w.IsSelected).ToList());
            }
        }

        private T _selectedItem;

        public T SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                SetValue(ref _selectedItem, value);
                OnPropertyChanged(nameof(SelectedItem));
                SelectedItem.ToEditPage();
            }
        }

        private string _itemsSearchQuery;

        public string ItemsSearchQuery
        {
            get { return _itemsSearchQuery; }
            set
            {
                SetValue(ref _itemsSearchQuery, value);
                OnPropertyChanged(nameof(ItemsSearchQuery));
                // TODO: исправить
                //IEnumerable<T> findedItems = _allItems;
                //findedItems = findedItems.Where(i => i.stringByWhichToFind.Contains(_itemsSearchQuery));
                //_items = new ObservableCollection<T>(findedItems);

                Items = new ObservableCollection<T>(AllItems.Where(i => i.StringByWhichToFind.Contains(_itemsSearchQuery)).ToList());

                //List<T> items = new List<T>();
                //foreach (var item in Items)
                //{
                //    if(item.stringByWhichToFind.Contains(_itemsSearchQuery))

                //}
            }
        }

        private bool _isMultiselect;

        public bool IsMultiselect
        {
            get { return _isMultiselect; }
            set
            {
                SetValue(ref _isMultiselect, value);
                OnPropertyChanged(nameof(IsMultiselect));
            }
        }

        protected async Task RefreshAsync()
        {
            ReadCollectionFromDb();
            Sort();
            AllItems = Items;
        }

        protected virtual void ReadCollectionFromDb()
        {
            //return Task.FromResult(default(List<T>));
        }

        protected virtual Task Sort()
        {
            return Task.FromResult(default(object));
        }

        protected async Task ItemTapped(T item)
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

        protected async Task DisplayCheckBoxes()
        {
            IsMultiselect = true;
        }

        protected async Task CancelMultiselect()
        {
            IsMultiselect = false;
            AllItems.ForEach(w => w.IsSelected = false);
        }

        protected virtual Task DeleteItems(object obj)
        {
            return Task.FromResult(default(object));
        }

        protected async Task SelectAll()
        {
            AllItems.ForEach(w => w.IsSelected = true);
        }
    }
}
