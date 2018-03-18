using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using System.Windows.Threading;

namespace PwC.SMART.Batch.Infrastructure
{
    /// <summary>
    /// Synchronized Observable SortedList Class
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class SynchronizedObservableSortedList<TKey, TValue> : ObservableSortedList<TKey, TValue>
    {
        /// <summary>
        /// The dispatcher object
        /// </summary>
        public Dispatcher Dispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedObservableSortedList&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        public SynchronizedObservableSortedList() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedObservableSortedList&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        /// <param name="count">The count.</param>
        /// <param name="comparer">The comparer.</param>
        public SynchronizedObservableSortedList(Dispatcher dispatcher = null, int count = 0,
                                                IComparer<TKey> comparer = null)
            : base(count, comparer)
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
        /// Called when [collection changed].
        /// </summary>
        /// <param name="action">The action.</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedAction action)
        {
            if (Dispatcher != null && Dispatcher.Thread != Thread.CurrentThread)
            {
                if (Dispatcher.Thread.ThreadState != ThreadState.Stopped && !Dispatcher.Thread.IsBackground)
                {
                    Dispatcher.Invoke(new Action(() => { base.OnCollectionChanged(action); }), null);
                }
                else
                {
                    Dispatcher.BeginInvoke(new Action(() => { base.OnCollectionChanged(action); }), null);
                }
            }
            else
            {
                base.OnCollectionChanged(action);
            }
        }
    }
}