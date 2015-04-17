using System;

namespace Utilities.Functional
{
    public sealed class Success<TSuccess, TFailure> : Result<TSuccess, TFailure>
    {
        private readonly TSuccess mValue;

        public override bool IsSuccess
        {
            get { return true; }
        }

        internal Success(TSuccess value)
        {
            mValue = value;
        }

        public override Result<R, TFailure> Select<R>(Func<TSuccess, R> selector)
        {
            return new Success<R, TFailure>( selector.Invoke(mValue));
        }

        public override TSuccess Get()
        {
            return mValue;
        }

        public override Result<R, TFailure> SelectMany<R>(Func<TSuccess, Result<R, TFailure>> selector)
        {
            return selector.Invoke(mValue);
        }

        public override Result<TSuccess, TFailure> Where(Func<TSuccess, bool> predicate, Func<TFailure> failure)
        {
            return predicate.Invoke(mValue) ? this : Fail(failure.Invoke());
        }

        public override void Iter(Action<TSuccess> action)
        {
            action.Invoke(mValue);
        }

        public override R Match<R>(Func<TSuccess, R> success, Func<TFailure, R> failure)
        {
            return success.Invoke(mValue);
        }

        public override TSuccess Or(Func<TSuccess> selector)
        {
            return mValue;
        }

        public override Result<TSuccess, TFailure> Or(Func<Result<TSuccess, TFailure>> selector)
        {
            return this;
        }

        public override Result<R, TFailure> OfType<R>(Func<TFailure> failure)
        {
            return mValue is R ? (Result<R, TFailure>) new Success<R, TFailure>((R)(object)mValue) 
                               : new Failure<R, TFailure>(failure.Invoke());
        }


    }
}
