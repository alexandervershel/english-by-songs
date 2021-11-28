using Dtos;
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
    public abstract class BaseListViewModel<T> : BaseViewModel where T : IListViewItemViewModel
    {
        private static readonly IServiceProvider _serviceProvider = ServiceProviderFactory.ServiceProvider;
        protected readonly IPageService _pageService;
        private ObservableCollection<T> _items;
        private ObservableCollection<T> _allItems;
        private T _selectedItem;
        private string _itemsSearchQuery;
        private bool _isMultiselect;
        public virtual ICommand ItemTappedCommand { get; private set; }
        public virtual ICommand DisplayCheckBoxesCommand { get; private set; }
        public virtual ICommand CancelMultiselectCommand { get; private set; }
        public virtual ICommand DeleteWordsCommand { get; private set; }
        public virtual ICommand SelectAllCommand { get; private set; }
        public BaseListViewModel()
        {
            _pageService = _serviceProvider.GetService<IPageService>();
            _isMultiselect = false;
            Items = new ObservableCollection<T>();
            ReadCollectionFromDb();
            Sort();
            AllItems = Items;

            DisplayCheckBoxesCommand = new Command(async () => await EnableMultiselect());
            CancelMultiselectCommand = new Command(async () => await DisableMultiselect());
            ItemTappedCommand = new Command<T>(async (item) => await ItemTapped(item));
            DeleteWordsCommand = new Command<object>(async (obj) => await DeleteItems(obj));
            SelectAllCommand = new Command(async () => await SelectAll());
            
        }

        public ObservableCollection<T> Items
        {
            get { return _items; }
            set
            {
                SetValue(ref _items, value);
                OnPropertyChanged(nameof(Items));
            }
        }

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

        public T SelectedItem
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

                Items = new ObservableCollection<T>(AllItems.Where(i => i.StringByWhichToFind.Contains(_itemsSearchQuery)).ToList());
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
