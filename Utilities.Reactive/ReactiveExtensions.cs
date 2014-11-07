using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;

namespace Utilities.Reactive
{
    public static class ReactiveExtensions
    {
        public static IObservable<TSource> ObserveOnUI<TSource>(this IObservable<TSource> source)
        {
            if (SynchronizationContext.Current == null)
            {
                return source.ObserveOn(Scheduler.CurrentThread);
            }
            return source.ObserveOn(SynchronizationContext.Current);
        }

        public static IObservable<TSource> SubscribeOnUI<TSource>(this IObservable<TSource> source)
        {
            if (SynchronizationContext.Current == null)
            {
                return source.SubscribeOn(Scheduler.CurrentThread);
            }
            return source.SubscribeOn(SynchronizationContext.Current);
        }

        public static IObservable<TSource> SelectArgs<TSource>(this IObservable<EventPattern<TSource>> source)
        {
            return source.Select(ev => ev.EventArgs);
        }

        public static IObservable<TSource> WhereIsNotNull<TSource>(this IObservable<TSource> source)
            where TSource : class
        {
            return source.Where(arg => arg != null);
        }

        public static IObservable<int> CountObservable<TSource>(this TSource source)
            where TSource : ICollection, INotifyCollectionChanged
        {
            return Observable
                .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                    h => source.CollectionChanged += h,
                    h => source.CollectionChanged -= h)
                .Select(_ => source.Count)
                .StartWith(source.Count);
        }

        public static IObservable<Unit> SelectUnit<TSource>(this IObservable<TSource> source)
        {
            return source.Select(_ => Unit.Default);
        }
    }
}