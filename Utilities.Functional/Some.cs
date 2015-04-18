using System;
using System.Collections.Generic;

namespace Utilities.Functional
{
    public sealed class Some<T> : Option<T>
    {
        private readonly T mValue;

        public override bool IsSome
        {
            get { return true;  }
        }

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

        public override R Match<R>(Func<T, R> some, Func<R> none)
        {
            return some.Invoke(mValue);
        }

        public override T Or(Func<T> selector)
        {
            return mValue;
        }

        public override Option<T> Or(Func<Option<T>> selector)
        {
            return this;
        }

        public override Option<R> OfType<R>()
        {
            return mValue is R ? new Some<R>((R)(object)mValue) : Option<R>.None; 
        }

        public override IEnumerable<T> ToEnumerable()
        {
            yield return mValue;
        }
    }
}
