using System;
using System.Collections.Generic;

namespace Utilities.Functional
{
    public struct Option<T>
    {
        public static readonly Option<T> None = new Option<T>();

        private readonly T _value;
        private readonly bool _isSome;

        public T GetUnsafe
        {
            get
            {
                if (IsSome)
                {
                    return _value;
                }
                else
                {
                    throw new InvalidOperationException("The option is None");
                }
            }
        }
        public bool IsSome { get { return _isSome; } }

        private Option(T value)
        {
            _isSome = value != null;
            _value = value;
        }

        public static Option<T> AsOption(T value)
        {
            return new Option<T>(value);
        }

        public static Option<OUT> Try<OUT>(Func<OUT> function)
        {
            try
            {
                return Option<OUT>.AsOption(function.Invoke());
            }
            catch
            {
                return Option<OUT>.None;
            }
        }


        public Option<OUT> Select<OUT>(Func<T, OUT> selector)
        {
            return IsSome
                ? Option<OUT>.AsOption(selector.Invoke(_value))
                : Option<OUT>.None;
        }

        public Option<OUT> SelectMany<OUT>(Func<T, Option<OUT>> selector)
        {
            return IsSome
                ? selector.Invoke(_value)
                : Option<OUT>.None;
        }

        public Option<T> Where(Func<T, bool> predicate)
        {
            return IsSome
                    ? predicate.Invoke(_value) ? this : None
                    : None;
        }

        public IEnumerable<T> ToEnumerable()
        {
            if (IsSome)
            {
                yield return _value;
            }
        }

        public OUT Match<OUT>(Func<T, OUT> some, Func<OUT> none)
        {
            return IsSome
                ? some.Invoke(_value)
                : none.Invoke();
        }

        public void Iter(Action<T> action)
        {
            if (IsSome)
            {
                action.Invoke(_value);
            }
        }

        public T OrDefault(Func<T> def)
        {
            return IsSome
                ? _value
                : def.Invoke();
        }

        public Option<T> Or(Func<Option<T>> def)
        {
            return IsSome
                ? this
                : def.Invoke();
        }

        public Option<OUT> OfType<OUT>()
        {
            return IsSome
                ? _value is OUT ? Option<OUT>.AsOption((OUT)(object)_value) : Option<OUT>.None
                : Option<OUT>.None;
        }

    }

}
