using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Utilities.Reactive;

namespace MapsW8.Base.Reactive
{
    internal class SingletonPropertyChangedEventArgs
    {
        public static readonly PropertyChangedEventArgs Value = new PropertyChangedEventArgs("Value");
    }

    internal class SingletonDataErrorsChangedEventArgs
    {
        public static readonly DataErrorsChangedEventArgs Value = new DataErrorsChangedEventArgs("Value");
    }

    [Flags]
    public enum ReactivePropertyMode
    {
        None = 0x00,

        /// <summary>If next value is same as current, not set and not notify.</summary>
        DistinctUntilChanged = 0x01,

        /// <summary>Push notify on instance created and subscribed.</summary>
        RaiseLatestValueOnSubscribe = 0x02
    }

    // for EventToReactive and Serialization
    public interface IReactiveProperty
    {
        object Value { get; }

        IObservable<IEnumerable> ObserveErrorChanged { get; }
    }

    /// <summary>
    /// Two-way bindable IObserable&lt;T&gt;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ReactivePropertyBase<T> : IObservable<T>, IDisposable, INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private T _latestValue;
        private bool _isDisposed = false;
        private readonly IScheduler _raiseEventScheduler;
        private readonly IObservable<T> _source;
        private readonly Subject<T> _anotherTrigger = new Subject<T>();
        private readonly IDisposable _sourceDisposable;
        private readonly IDisposable _raiseSubscription;

        // for Validation
        private bool _isValueChanged = false;

        private readonly SerialDisposable _validateNotifyErrorSubscription = new SerialDisposable();
        private readonly Subject<IEnumerable> _errorsTrigger = new Subject<IEnumerable>();
        private List<Func<IObservable<T>, IObservable<IEnumerable>>> _validatorStore = new List<Func<IObservable<T>, IObservable<IEnumerable>>>();

        /// <summary>PropertyChanged raise on UIDispatcherScheduler</summary>
        protected ReactivePropertyBase()
            : this(default(T), ReactivePropertyMode.DistinctUntilChanged | ReactivePropertyMode.RaiseLatestValueOnSubscribe)
        {
        }

        /// <summary>PropertyChanged raise on UIDispatcherScheduler</summary>
        protected ReactivePropertyBase(T initialValue = default(T), ReactivePropertyMode mode = ReactivePropertyMode.DistinctUntilChanged|ReactivePropertyMode.RaiseLatestValueOnSubscribe)
            : this(UIDispatcherScheduler.Default, initialValue, mode)
        { }

        /// <summary>PropertyChanged raise on selected scheduler</summary>
        protected ReactivePropertyBase(IScheduler raiseEventScheduler, T initialValue = default(T), ReactivePropertyMode mode = ReactivePropertyMode.DistinctUntilChanged|ReactivePropertyMode.RaiseLatestValueOnSubscribe)
            : this(System.Reactive.Linq.Observable.Never<T>(), raiseEventScheduler, initialValue, mode)
        {
        }

        // ToReactiveProperty Only
        internal ReactivePropertyBase(IObservable<T> source, T initialValue = default(T), ReactivePropertyMode mode = ReactivePropertyMode.DistinctUntilChanged|ReactivePropertyMode.RaiseLatestValueOnSubscribe)
            : this(source, UIDispatcherScheduler.Default, initialValue, mode)
        {
        }

        internal ReactivePropertyBase(IObservable<T> source, IScheduler raiseEventScheduler, T initialValue = default(T), ReactivePropertyMode mode = ReactivePropertyMode.DistinctUntilChanged|ReactivePropertyMode.RaiseLatestValueOnSubscribe)
        {
            _latestValue = initialValue;
            _raiseEventScheduler = raiseEventScheduler;

            // create source
            var merge = source.Merge(_anotherTrigger);
            if (mode.HasFlag(ReactivePropertyMode.DistinctUntilChanged)) merge = merge.DistinctUntilChanged();
            merge = merge.Do(x =>
            {
                // setvalue immediately
                if (!_isValueChanged) _isValueChanged = true;
                _latestValue = x;
            });

            // publish observable
            var connectable = (mode.HasFlag(ReactivePropertyMode.RaiseLatestValueOnSubscribe))
                ? merge.Publish(initialValue)
                : merge.Publish();
            _source = connectable.AsObservable();

            // raise notification
            _raiseSubscription = connectable
                .Where(_ => PropertyChanged != null)
                .ObserveOn(raiseEventScheduler)
                .Subscribe(x =>
                {
                    var handler = PropertyChanged;
                    if (handler != null)
                    {
                        handler(this, SingletonPropertyChangedEventArgs.Value);
                    }
                });

            // start source
            _sourceDisposable = connectable.Connect();
        }

        /// <summary>
        /// Get latestValue or push(set) value.
        /// </summary>
        protected T InternalValue
        {
            get { return _latestValue; }
            set
            {
                if (!_isDisposed)
                {
                    _anotherTrigger.OnNext(value);
                }
            }
        }

        /// <summary>
        /// Subscribe source.
        /// </summary>
        public IDisposable Subscribe(IObserver<T> observer)
        {
            return _source.Subscribe(observer);
        }

        /// <summary>
        /// Unsubcribe all subscription.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed) return;

            _isDisposed = true;
            _anotherTrigger.Dispose();
            _raiseSubscription.Dispose();
            _sourceDisposable.Dispose();
            _validateNotifyErrorSubscription.Dispose();
            _errorsTrigger.OnCompleted();
            _errorsTrigger.Dispose();
        }

        public override string ToString()
        {
            return (_latestValue == null)
                ? "null"
                : "{" + _latestValue.GetType().Name + ":" + _latestValue.ToString() + "}";
        }

        // Validations

        /// <summary>
        /// <para>Checked validation, raised value. If success return value is null.</para>
        /// </summary>
        public IObservable<IEnumerable> ObserveErrorChanged
        {
            get { return _errorsTrigger.AsObservable(); }
        }

        // INotifyDataErrorInfo

        private IEnumerable _currentErrors;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// <para>Set INotifyDataErrorInfo's asynchronous validation, return value is self.</para>
        /// </summary>
        /// <param name="validator">If success return IO&lt;null&gt;, failure return IO&lt;IEnumerable&gt;(Errors).</param>
        /// <returns>Self.</returns>
        public ReactivePropertyBase<T> SetValidateNotifyError(Func<IObservable<T>, IObservable<IEnumerable>> validator)
        {
            _validatorStore.Add(validator);		//--- cache validation functions
            var validators = _validatorStore
                            .Select(x => x(this._source))
                            .ToArray();		//--- use copy
            _validateNotifyErrorSubscription.Disposable
                = System.Reactive.Linq.Observable.CombineLatest(validators)
                .Select(xs =>
                {
                    if (xs.Count == 0) return null;
                    if (xs.All(x => x == null)) return null;

                    var strings = xs
                                .OfType<string>()
                                .Where(x => x != null);
                    var others = xs
                                .Where(x => !(x is string))
                                .Where(x => x != null)
                                .SelectMany(x => x.Cast<object>());
                    return strings.Concat(others);
                })
                .Subscribe(x =>
                {
                    _currentErrors = x;
                    var handler = this.ErrorsChanged;
                    if (handler != null)
                        _raiseEventScheduler.Schedule(() => handler(this, SingletonDataErrorsChangedEventArgs.Value));
                    _errorsTrigger.OnNext(x);
                });
            return this;
        }

        /// <summary>
        /// <para>Set INotifyDataErrorInfo's asynchronous validation, return value is self.</para>
        /// </summary>
        /// <param name="validator">If success return IO&lt;null&gt;, failure return IO&lt;IEnumerable&gt;(Errors).</param>
        /// <returns>Self.</returns>
        public ReactivePropertyBase<T> SetValidateNotifyError(Func<IObservable<T>, IObservable<string>> validator)
        {
            return this.SetValidateNotifyError(xs => validator(xs).Cast<IEnumerable>());
        }

        /// <summary>
        /// Set INotifyDataErrorInfo's asynchronous validation.
        /// </summary>
        /// <param name="validator">Validation logic</param>
        /// <returns>Self.</returns>
        public ReactivePropertyBase<T> SetValidateNotifyError(Func<T, Task<IEnumerable>> validator)
        {
            return this.SetValidateNotifyError(xs => xs.SelectMany(x => validator(x)));
        }

        /// <summary>
        /// Set INotifyDataErrorInfo's asynchronous validation.
        /// </summary>
        /// <param name="validator">Validation logic</param>
        /// <returns>Self.</returns>
        public ReactivePropertyBase<T> SetValidateNotifyError(Func<T, Task<string>> validator)
        {
            return this.SetValidateNotifyError(xs => xs.SelectMany(x => validator(x)));
        }

        /// <summary>
        /// Set INofityDataErrorInfo validation.
        /// </summary>
        /// <param name="validator">Validation logic</param>
        /// <returns>Self.</returns>
        public ReactivePropertyBase<T> SetValidateNotifyError(Func<T, IEnumerable> validator)
        {
            return this.SetValidateNotifyError(xs => xs.Select(x => validator(x)));
        }

        /// <summary>
        /// Set INofityDataErrorInfo validation.
        /// </summary>
        /// <param name="validator">Validation logic</param>
        /// <returns>Self.</returns>
        public ReactivePropertyBase<T> SetValidateNotifyError(Func<T, string> validator)
        {
            return this.SetValidateNotifyError(xs => xs.Select(x => validator(x)));
        }

        /// <summary>Get INotifyDataErrorInfo's error store</summary>
        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            return _currentErrors;
        }

        /// <summary>Get INotifyDataErrorInfo's error store</summary>
        public bool HasErrors
        {
            get { return _currentErrors != null; }
        }
    }
}