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

        public abstract R Match<R>(Func<T, R> some, Func<R> none);

        public abstract T Or(Func<T> selector);

        public static Option<T> AsOption(T value)
        {
            return value != null ? new Some<T>(value) : None;
        }

        public static Option<T> Try(Func<T> selector)
        {
            try
            {
                return new Some<T>(selector.Invoke());
            }
            catch
            {
                return None;
            }
        }
    }
}
