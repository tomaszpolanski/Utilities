using System;

namespace Utilities.Functional
{
    public abstract class Option<T>
    {
        public static readonly Option<T> None = new None<T>();

        public abstract Option<R> Select<R>(Func<T, R> selector);

        public abstract Option<R> SelectMany<R>(Func<T, Option<R>> selector);

        public abstract Option<T> Where(Func<T, bool> predicate);

        public abstract T Get();

        public abstract void Iter(Action<T> action);

        public static Option<T> AsOption(T value)
        {
            return value != null ? new Some<T>(value) : None;
        }
    }
}
