using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Functional
{
    public sealed class Some<T> : Option<T>
    {
        private readonly T mValue;

        internal Some(T value)
        {
            mValue = value;
        }

        public override Option<R> Bind<R>(Func<T, Option<R>> selector)
        {
            return selector.Invoke(mValue);
        }

        public override T Get()
        {
            return mValue;
        }

        public override Option<R> Map<R>(Func<T, R> selector)
        {
            return new Some<R>(selector.Invoke(mValue));
        }
    }
}
