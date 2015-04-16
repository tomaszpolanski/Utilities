using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Functional
{
    public abstract class Option<T>
    {
        public static readonly Option<T> None = new None<T>();

        public abstract Option<R> Map<R>(Func<T, R> selector);

        public abstract Option<R> Bind<R>(Func<T, Option<R>> selector);

        public abstract T Get();

        public static Option<T> AsOption(T value)
        {
            return value != null ? new Some<T>(value) : None;
        }
    }
}
