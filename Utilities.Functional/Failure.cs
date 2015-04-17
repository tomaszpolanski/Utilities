using System;

namespace Utilities.Functional
{
    public sealed class Failure<TSuccess, TFailure> : Result<TSuccess, TFailure>
    {
        private readonly TFailure mMessage;

        public override bool IsSuccess
        {
            get { return false; }
        }

        internal Failure(TFailure message)
        {
            mMessage = message;
        }

        public override Result<R, TFailure> Select<R>(Func<TSuccess, R> selector)
        {
            return new Failure<R, TFailure>(mMessage);
        }

        public override Result<R, TFailure> SelectMany<R>(Func<TSuccess, Result<R, TFailure>> selector)
        {
            return new Failure<R, TFailure>(mMessage);
        }

        public override TSuccess Get()
        {
            throw new NotSupportedException();
        }

        public override Result<TSuccess, TFailure> Where(Func<TSuccess, bool> predicate, Func<TFailure> failure)
        {
            return this;
        }

        public override void Iter(Action<TSuccess> action)
        {
        }

        public override R Match<R>(Func<TSuccess, R> success, Func<TFailure, R> failure)
        {
            return failure.Invoke(mMessage);
        }

        public override TSuccess Or(Func<TSuccess> selector)
        {
            return selector.Invoke();
        }

        public override Result<TSuccess, TFailure> Or(Func<Result<TSuccess, TFailure>> selector)
        {
            return selector.Invoke();
        }

        public override Result<R, TFailure> OfType<R>(Func<TFailure> failure)
        {
            return new Failure<R, TFailure>(mMessage);
        }


    }
}
