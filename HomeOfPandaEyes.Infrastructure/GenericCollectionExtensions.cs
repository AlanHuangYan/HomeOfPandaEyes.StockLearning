using System;
using System.Collections.Generic;
using System.Collections;

namespace HomeOfPandaEyes.Infrastructure
{
    public static class GenericCollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection,
                                       params T[] items)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            foreach (T t in items)
            {
                collection.Add(t);
            }
        }

        public static void AddRange<T>(this ICollection<T> collection,
                                       IEnumerable<T> items)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            foreach (T item in items)
            {
                collection.Add(item);
            }
        }

        public static void AddRange<T>(this ICollection<T> collection, ICollection items)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            foreach (object item in items)
            {
                collection.Add((T)item);
            }
        }
    }
}