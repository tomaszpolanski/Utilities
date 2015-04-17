using System;

namespace Utilities.Functional
{
    public abstract class Result<TSuccess, TFailure>
    {

        public abstract bool IsSuccess { get; }

        public abstract Result<R, TFailure> Select<R>(Func<TSuccess, R> selector);

        public abstract Result<R, TFailure> SelectMany<R>(Func<TSuccess, Result<R, TFailure>> selector);

        public abstract Result<TSuccess, TFailure> Where(Func<TSuccess, bool> predicate, Func<TFailure> failure);

        public abstract void Iter(Action<TSuccess> action);

        public abstract R Match<R>(Func<TSuccess, R> success, Func<TFailure, R> failure);

        public abstract TSuccess Or(Func<TSuccess> selector);

        public abstract Result<TSuccess, TFailure> Or(Func<Result<TSuccess, TFailure>> selector);

        public abstract Result<R, TFailure> OfType<R>(Func<TFailure> failure);

        public abstract TSuccess Get(); 


        public static Result<TSuccess, TFailure> AsResult(TSuccess success, TFailure failure)
        {
            return success != null ? (Result<TSuccess, TFailure>)new Success<TSuccess, TFailure>(success) : new Failure<TSuccess, TFailure>(failure);
        }

        public static Result<TSuccess, TFailure> Fail(TFailure failure)
        {
            return new Failure<TSuccess, TFailure>(failure);
        }

        public static Result<TSuccess, TFailure> Try(Func<TSuccess> selector, Func<Exception, TFailure> failure)
        {
            try
            {
                return new Success<TSuccess, TFailure>(selector.Invoke());
            }
            catch (Exception e)
            {
                return Fail(failure.Invoke(e));
            }
        }
    }
}
