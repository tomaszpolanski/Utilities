using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.Functional
{
    public static class LinqExtensions
    {

        public static IEnumerable<U> Choose<T, U>(this IEnumerable<T> self, Func<T, Option<U>> selector)
        {
            return self.Select(selector)
                       .Where(option => option.IsSome)
                       .Select(option => option.Get());
        }

        public static Option<T> TryFind<T>(this IEnumerable<T> self, Func<T, bool> predicate)
        {
            var list = self.Where(predicate)
                           .Take(1)
                           .ToList();

            return list.Count == 1 ? Option<T>.AsOption(list.FirstOrDefault()) : Option<T>.None;
        }

        public static Option<T> Get<T>(this IEnumerable<T> self, int index)
        {
            try
            {
                return Option<T>.AsOption(self.ElementAt(index));
            }
            catch
            {
                return Option<T>.None;
            }
        }
    }
}
