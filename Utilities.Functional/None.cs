using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.Functional
{
    public sealed class None<T> : Option<T>
    {

        public override bool IsSome
        {
            get { return false; }
        }

        internal None() { }

        public override bool Equals(object obj) 
        {
            return obj is None<T>;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override T GetUnsafe
        {
            get { throw new NotSupportedException(); }
        }

        public override Option<R> Select<R>(Func<T, R> selector)
        {
            return Option<R>.None;
        }

        public override Option<R> SelectMany<R>(Func<T, Option<R>> selector)
        {
            return Option<R>.None;
        }

        public override Option<T> Where(Func<T, bool> predicate)
        {
            return None;
        }

        public override void Iter(Action<T> action)
        {
        }

        public override R Match<R>(Func<T, R> some, Func<R> none)
        {
            return none.Invoke();
        }

        public override T Or(Func<T> selector)
        {
            return selector.Invoke();
        }

        public override Option<T> Or(Func<Option<T>> selector)
        {
            return selector.Invoke();
        }

        public override Option<R> OfType<R>()
        {
            return Option<R>.None;
        }

        public override IEnumerable<T> ToEnumerable()
        {
            return Enumerable.Empty<T>();
        }
    }
}
