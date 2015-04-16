using System;

namespace Utilities.Functional
{
    public sealed class Some<T> : Option<T>
    {
        private readonly T mValue;

        internal Some(T value)
        {
            mValue = value;
        }

        public override Option<R> SelectMany<R>(Func<T, Option<R>> selector)
        {
            return selector.Invoke(mValue);
        }

        public override T Get()
        {
            return mValue;
        }

        public override Option<R> Select<R>(Func<T, R> selector)
        {
            return new Some<R>(selector.Invoke(mValue));
        }

        public override Option<T> Where(Func<T, bool> predicate)
        {
            return predicate.Invoke(mValue) ? this : None;
        }

        public override void Iter(Action<T> action)
        {
            action.Invoke(mValue);
        }
    }
}
