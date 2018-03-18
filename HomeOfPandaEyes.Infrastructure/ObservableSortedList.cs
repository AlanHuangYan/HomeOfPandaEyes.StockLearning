using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace PwC.SMART.Batch.Infrastructure
{
    /// <summary>
    /// ObservableSortedList Class
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    [Serializable]
    public class ObservableSortedList<TKey, TValue> : IDictionary<TKey, TValue>, INotifyCollectionChanged,
                                                      INotifyPropertyChanged, IXmlSerializable
    {
        private readonly object _lockObject = new object();
        private readonly SortedList<TKey, TValue> _sortedList;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableSortedList&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        public ObservableSortedList()
        {
            _sortedList = new SortedList<TKey, TValue>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableSortedList&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <param name="comparer">The comparer.</param>
        public ObservableSortedList(int capacity = 0, IComparer<TKey> comparer = null)
        {
            if (comparer != null)
            {
                if (capacity != 0)
                {
                    _sortedList = new SortedList<TKey, TValue>(capacity, comparer);
                }
                else
                {
                    _sortedList = new SortedList<TKey, TValue>(comparer);
                }
            }
            else
            {
                if (capacity != 0)
                {
                    _sortedList = new SortedList<TKey, TValue>(capacity);
                }
                else
                {
                    _sortedList = new SortedList<TKey, TValue>();
                }
            }
        }

        #region Dictionary methods

        /// <summary>
        /// Adds the specified key and value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(TKey key, TValue value)
        {
            lock (_lockObject)
            {
                _sortedList.Add(key, value);
                OnCollectionChanged(NotifyCollectionChangedAction.Add);
            }
        }

        /// <summary>
        /// Determines whether the sorted list contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsKey(TKey key)
        {
            return _sortedList.ContainsKey(key);
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        public ICollection<TKey> Keys
        {
            get { return _sortedList.Keys; }
        }

        /// <summary>
        /// Removes the specified object by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///     <c>true</c> if the specified key contains; otherwise, <c>false</c>.
        /// </returns>
        public bool Remove(TKey key)
        {
            lock (_lockObject)
            {
                if (_sortedList.Remove(key))
                {
                    OnCollectionChanged(NotifyCollectionChangedAction.Remove);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     <c>true</c> if the specified key contains; otherwise, <c>false</c>.
        /// </returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return _sortedList.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        public ICollection<TValue> Values
        {
            get
            {
                //lock (_lockObject)
                {
                    return _sortedList.Values.ToArray();
                }
            }
        }

        /// <summary>
        /// Gets or sets the TValue with the specified key.
        /// </summary>
        public TValue this[TKey key]
        {
            get
            {
                if (_sortedList.ContainsKey(key))
                {
                    return _sortedList[key];
                }

                throw new InvalidOperationException("Can't load key " + key);
            }
            set
            {
                _sortedList[key] = value;

                OnCollectionChanged(NotifyCollectionChangedAction.Replace);
            }
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            lock (_lockObject)
            {
                _sortedList.Add(item.Key, item.Value);
                OnCollectionChanged(NotifyCollectionChangedAction.Add);
            }
        }

        /// <summary>
        /// Clears sorted list.
        /// </summary>
        public void Clear()
        {
            lock (_lockObject)
            {
                _sortedList.Clear();
                OnCollectionChanged(NotifyCollectionChangedAction.Reset);
            }
        }

        /// <summary>
        /// Determines whether contains the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if contains the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _sortedList.Contains(item);
        }

        /// <summary>
        /// Copy to array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>) _sortedList).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the count of sorted list elements.
        /// </summary>
        public int Count
        {
            get { return _sortedList.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether this sorted list instance is read only.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///     <c>true</c> if contains the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            lock (_lockObject)
            {
                if (_sortedList.Remove(item.Key))
                {
                    OnCollectionChanged(NotifyCollectionChangedAction.Remove);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>Enumerator</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _sortedList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _sortedList.GetEnumerator();
        }

        /// <summary>
        /// Determines whether the sorted list contains value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the sorted list contains value; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsValue(TValue value)
        {
            return Values.Contains(value);
        }

        /// <summary>
        /// Removes the item by value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the sorted list contains value; otherwise, <c>false</c>.</returns>
        public TKey RemoveByValue(TValue value)
        {
            lock (_lockObject)
            {
                if (ContainsValue(value))
                {
                    foreach (var item in _sortedList)
                    {
                        if (item.Value.Equals(value))
                        {
                            if (Remove(item.Key))
                            {
                                return item.Key;
                            }
                        }
                    }
                }
            }

            return default(TKey);
        }

        #endregion

        #region Serialization

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"/> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/> method.
        /// </returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            var keySerializer = new XmlSerializer(typeof (TKey));
            var valueSerializer = new XmlSerializer(typeof (TValue));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");

                reader.ReadStartElement("key");
                var key = (TKey) keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("value");
                var value = (TValue) valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                Add(key, value);

                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public void WriteXml(XmlWriter writer)
        {
            var keySerializer = new XmlSerializer(typeof (TKey));
            var valueSerializer = new XmlSerializer(typeof (TValue));

            foreach (TKey key in Keys)
            {
                writer.WriteStartElement("item");

                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();

                writer.WriteStartElement("value");
                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }

        #endregion

        #region INotifyCollectionChanged Members

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        /// <summary>
        /// Called when collection changed.
        /// </summary>
        /// <param name="action">The action.</param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedAction action)
        {
            if (CollectionChanged != null)
            {
                if (CollectionChanged.Target is ICollectionView)
                {
                    CollectionChanged(this, new NotifyCollectionChangedEventArgs(
                                                NotifyCollectionChangedAction.Reset
                                                ));
                }
                else
                {
                    try
                    {
                        CollectionChanged(this, new NotifyCollectionChangedEventArgs(
                                                    action));
                    }
                    catch (ArgumentException)
                    {
                        CollectionChanged(this, new NotifyCollectionChangedEventArgs(
                                                    NotifyCollectionChangedAction.Reset
                                                    ));
                    }
                }
            }

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Keys"));
                PropertyChanged(this, new PropertyChangedEventArgs("Values"));

                if (action == NotifyCollectionChangedAction.Add ||
                    action == NotifyCollectionChangedAction.Remove ||
                    action == NotifyCollectionChangedAction.Reset)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Count"));
                    PropertyChanged(this, new PropertyChangedEventArgs("Item[]"));
                }
            }
        }
    }
}