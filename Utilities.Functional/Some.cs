using System;
using System.Collections.Generic;

namespace Utilities.Functional
{
    public sealed class Some<T> : Option<T>
    {
        private readonly T _value;

        public override bool IsSome
        {
            get { return true;  }
        }

        internal Some(T value)
        {
            _value = value;
        }

        public override Option<R> SelectMany<R>(Func<T, Option<R>> selector)
        {
            return selector.Invoke(_value);
        }

        public override T GetUnsafe
        {
            get { return _value; }
        }

        public override Option<R> Select<R>(Func<T, R> selector)
        {
            return Option<R>.AsOption(selector.Invoke(_value));
        }

        public override Option<T> Where(Func<T, bool> predicate)
        {
            return predicate.Invoke(_value) ? this : None;
        }

        public override void Iter(Action<T> action)
        {
            action.Invoke(_value);
        }

        public override R Match<R>(Func<T, R> some, Func<R> none)
        {
            return some.Invoke(_value);
        }

        public override T Or(Func<T> selector)
        {
            return _value;
        }

        public override Option<T> Or(Func<Option<T>> selector)
        {
            return this;
        }

        public override Option<R> OfType<R>()
        {
            return _value is R ? Option<R>.AsOption((R)(object)_value) : Option<R>.None; 
        }

        public override IEnumerable<T> ToEnumerable()
        {
            yield return _value;
        }

        public override T OrDefault(Func<T> selector)
        {
            return _value;
        }
    }
}
