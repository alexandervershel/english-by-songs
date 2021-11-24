using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Services.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void Sort<T>(this ObservableCollection<T> collection, Comparison<T> comparison)
        {
            List<T> listToSort = collection.ToList();
            listToSort.Sort(comparison);
            collection = new ObservableCollection<T>(listToSort);
        }
    }
}
