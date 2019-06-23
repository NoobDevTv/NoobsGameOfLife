using System;
using System.Collections.Generic;
using System.Text;

namespace NoobsGameOfLife.Core.Extensions
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }

            return enumerable;
        }

        public static IEnumerable<T> OnEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
                yield return item;
            }
        }
    }
}
