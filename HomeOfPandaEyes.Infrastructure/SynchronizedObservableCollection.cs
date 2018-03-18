using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading;
using System.Windows.Threading;

namespace PwC.SMART.Batch.Infrastructure
{
    /// <summary>
    /// Synchronized Observable Collection Class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SynchronizedObservableCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        /// The dispatcher object
        /// </summary>
        public Dispatcher Dispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedObservableCollection&lt;T&gt;"/> class.
        /// </summary>
        public SynchronizedObservableCollection()
        {
            InitCollection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedObservableCollection&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        public SynchronizedObservableCollection(IEnumerable<T> items, Dispatcher dispatcher = null) : base(items)
        {
            InitCollection(null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedObservableCollection&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        public SynchronizedObservableCollection(Dispatcher dispatcher)
        {
            InitCollection(dispatcher);
        }

        private void InitCollection(Dispatcher dispatcher = null)
        {
            if (dispatcher == null)
            {
                Dispatcher = Dispatcher.CurrentDispatcher;
            }
            else
            {
                Dispatcher = dispatcher;
            }
        }

        /// <summary>
        /// Inserts the range.
        /// </summary>
        /// <param name="range">The range.</param>
        public void InsertRange(IEnumerable<T> range)
        {
            foreach (T item in range)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:CollectionChanged"/> event.
        /// </summary>
        /// <param name="action">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs action)
        {
            if (Dispatcher.Thread != Thread.CurrentThread)
            {
                if (Dispatcher.Thread.ThreadState != ThreadState.Stopped && !Dispatcher.Thread.IsBackground)
                {
                    Dispatcher.Invoke(new Action(() => { ChangeCollectionByAction(action); }), null);
                }
                else
                {
                    Dispatcher.BeginInvoke(new Action(() => { ChangeCollectionByAction(action); }), null);
                }
            }
            else
            {
                ChangeCollectionByAction(action);
            }
        }

        /// <summary>
        /// Changes the collection by action.
        /// </summary>
        /// <param name="action">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void ChangeCollectionByAction(NotifyCollectionChangedEventArgs action)
        {
            try
            {
                base.OnCollectionChanged(action);
            }
            catch
            {
                try
                {
                    base.OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                                                 NotifyCollectionChangedAction.Reset));
                }
                catch
                {
                }
            }
        }
    }
}