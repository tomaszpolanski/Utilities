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
    }
}
