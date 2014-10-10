using System;
using System.Reactive.Concurrency;
using MapsW8.Base.Reactive;

namespace Utilities.Reactive
{
    public static class ReactivePropertyExtensions
    {
        public static ReactiveProperty<T> ToReactiveProperty<T>(this IObservable<T> source,
            T initialValue = default(T),
            ReactivePropertyMode mode =
                ReactivePropertyMode.DistinctUntilChanged | ReactivePropertyMode.RaiseLatestValueOnSubscribe)
        {
            return new ReactiveProperty<T>(source, initialValue, mode);
        }

        public static ReactiveProperty<T> ToReactiveProperty<T>(this IObservable<T> source,
            IScheduler raiseEventScheduler,
            T initialValue = default(T),
            ReactivePropertyMode mode =
                ReactivePropertyMode.DistinctUntilChanged | ReactivePropertyMode.RaiseLatestValueOnSubscribe)
        {
            return new ReactiveProperty<T>(source, raiseEventScheduler, initialValue, mode);
        }

        public static ReadonlyReactiveProperty<T> ToReadonlyReactiveProperty<T>(this IObservable<T> source,
            T initialValue = default(T),
            ReactivePropertyMode mode =
                ReactivePropertyMode.DistinctUntilChanged | ReactivePropertyMode.RaiseLatestValueOnSubscribe)
        {
            return new ReadonlyReactiveProperty<T>(source, initialValue, mode);
        }

        public static ReadonlyReactiveProperty<T> ToReadonlyReactiveProperty<T>(this IObservable<T> source,
            IScheduler raiseEventScheduler,
            T initialValue = default(T),
            ReactivePropertyMode mode =
                ReactivePropertyMode.DistinctUntilChanged | ReactivePropertyMode.RaiseLatestValueOnSubscribe)
        {
            return new ReadonlyReactiveProperty<T>(source, raiseEventScheduler, initialValue, mode);
        }
    }
}