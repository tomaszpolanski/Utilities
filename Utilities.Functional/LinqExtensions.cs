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
                       .Select(option => option.GetUnsafe);
        }

        public static Option<T> TryFirst<T>(this IEnumerable<T> self, Func<T, bool> predicate)
        {
            return Option<T>.Try(() => self.First(predicate));
        }

        public static Option<T> TryFirst<T>(this IEnumerable<T> self)
        {
            return self.TryFirst(_ => true);
        }

        public static Option<T> TryLast<T>(this IEnumerable<T> self, Func<T, bool> predicate)
        {
            return Option<T>.Try(() => self.Last(predicate));
        }

        public static Option<T> TryLast<T>(this IEnumerable<T> self)
        {
            return self.TryLast(_ => true);
        }

        public static Option<T> TryFind<T>(this IEnumerable<T> self, Func<T, bool> predicate)
        {
            var list = self.Where(predicate)
                           .Take(1)
                           .ToList();

            return list.Count == 1 ? Option<T>.AsOption(list.FirstOrDefault()) : Option<T>.None;
        }

        public static Option<T> TryAggregate<T>(this IEnumerable<T> self, Func<T, T, T> aggregator)
        {
            return Option<T>.Try(() => self.Aggregate(aggregator));
        }

        public static Option<T> TryElementAt<T>(this IEnumerable<T> self, int index)
        {
            return Option<T>.Try(() => self.ElementAt(index));
        }
    }
}
