using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Functional
{
    public sealed class None<T> : Option<T>, IEquatable<T>
    {
        internal None() { }

        public bool Equals(T other)
        {
            return true;
        }

        public override bool Equals(object obj) 
        {
            return obj is None<T>;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override T Get()
        {
            throw new NotSupportedException();
        }

        public override Option<R> Map<R>(Func<T, R> selector)
        {
            return Option<R>.None;
        }

        public override Option<R> Bind<R>(Func<T, Option<R>> selector)
        {
            return Option<R>.None;
        }
    }
}
