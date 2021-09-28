using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DiaryApp.Extensions
{
    public static class EnumerableExtensions
    {
        public static void AddRange<T>(this IList<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> list, System.Threading.Tasks.Task<IList<Model.Notes>> notes)
        {
            var collection = new ObservableCollection<T>();
            foreach (var item in list) collection.Add(item);
            return collection;
        }

    }
}
