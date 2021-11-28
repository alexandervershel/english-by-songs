using System.Collections.Generic;
using System.Linq;

namespace EnglishBySongs.Helpers
{
    public static class CompareHelper
    {
        public static int CompareByStringValue<T>(IEnumerable<T> list1, IEnumerable<T> list2)
        {
            if (!list1.Any() && list2.Any())
            {
                return -1;
            }

            if (list1.Any() && !list2.Any())
            {
                return 1;
            }

            return string.Join("", list1.Select(t => t.ToString())).CompareTo(string.Join("", list2.Select(t => t.ToString())));
        }
    }
}
