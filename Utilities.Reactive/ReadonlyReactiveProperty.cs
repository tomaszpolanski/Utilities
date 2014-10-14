using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Utilities.Reactive
{
    public class ReadonlyReactiveProperty<T> : ReactivePropertyBase<T>, IReactiveProperty
    {
        /// <summary>
        /// Get latestValue or push(set) value.
        /// </summary>
        public T Value
        {
            get { return InternalValue; }
            private set { InternalValue = value; }
        }

        object IReactiveProperty.Value
        {
            get { return Value; }
        }

        /// <summary>PropertyChanged raise on UIDispatcherScheduler</summary>
        public ReadonlyReactiveProperty()
            : this(
                default(T), ReactivePropertyMode.DistinctUntilChanged | ReactivePropertyMode.RaiseLatestValueOnSubscribe
                )
        {
        }

        /// <summary>PropertyChanged raise on UIDispatcherScheduler</summary>
        public ReadonlyReactiveProperty(T initialValue = default(T),
            ReactivePropertyMode mode =
                ReactivePropertyMode.DistinctUntilChanged | ReactivePropertyMode.RaiseLatestValueOnSubscribe)
            : this(UIDispatcherScheduler.Default, initialValue, mode)
        {
        }

        /// <summary>PropertyChanged raise on selected scheduler</summary>
        public ReadonlyReactiveProperty(IScheduler raiseEventScheduler,
            T initialValue = default(T),
            ReactivePropertyMode mode =
                ReactivePropertyMode.DistinctUntilChanged | ReactivePropertyMode.RaiseLatestValueOnSubscribe)
            : this(Observable.Never<T>(), raiseEventScheduler, initialValue, mode)
        {
        }

        // ToReactiveProperty Only
        internal ReadonlyReactiveProperty(IObservable<T> source,
            T initialValue = default(T),
            ReactivePropertyMode mode =
                ReactivePropertyMode.DistinctUntilChanged | ReactivePropertyMode.RaiseLatestValueOnSubscribe)
            : this(source, UIDispatcherScheduler.Default, initialValue, mode)
        {
        }

        internal ReadonlyReactiveProperty(IObservable<T> source,
            IScheduler raiseEventScheduler,
            T initialValue = default(T),
            ReactivePropertyMode mode =
                ReactivePropertyMode.DistinctUntilChanged | ReactivePropertyMode.RaiseLatestValueOnSubscribe) :
                    base(source, raiseEventScheduler, initialValue, mode)
        {
        }
    }
}