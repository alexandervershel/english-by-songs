using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EnglishBySongs
{
    interface IStore<T>
    {
        Task<ICollection<T>> GetCollectionAsync();

        Task Get(int id);

        Task Add(T value);

        Task Update(T value);

        Task Remove(T value);
    }
}
