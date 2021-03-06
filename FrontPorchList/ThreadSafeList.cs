﻿using System.Collections;
using System.Collections.Generic;

namespace FrontPorchList
{
    /// <summary>
    /// My implementation of a generic list, which is pretty much a wrapper for just the standard list implementations, 
    /// but it locks on an object before attempting to do a data changing access the list to make sure that the contents are always valid.
    /// 
    /// What I think I'd really do is use SynchronizedCollection<T>, cause it implements IList<T> also, and it's part of the framework, 
    /// and is probably pretty good.
    /// </summary>
    /// <typeparam name="T">Generic type parameter</typeparam>
    public class ThreadSafeList<T> : IList<T>
    {
        // Internal list that actually holds the content
        private readonly IList<T> list;
        // Object used for locking prior to accessing the list.
        private readonly object lockObject;

        public ThreadSafeList()
        {
            list = new List<T>();
            lockObject = new object();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList) list).GetEnumerator();
        }

        /// <summary>
        /// Locks and waits, and adds an item to the intenal list, when clear.
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            lock (lockObject)
            {
                list.Add(item);
            }
        }


        /// <summary>
        /// Locks and clears the list.
        /// </summary>
        public void Clear()
        {
            lock (lockObject)
            {
                list.Clear();
            }
        }

        /// <summary>
        /// Locks and then calls the Contains method on the internal collection
        /// </summary>
        /// <param name="item">The item to test for to see if it exists.</param>
        /// <returns>bool if the internal collection contains the object</returns>
        public bool Contains(T item)
        {
            return list.Contains(item);
        }


        /// <summary>
        /// Locks and then calls the CopyTo method on the internal collection
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (lockObject)
            {
                list.CopyTo(array, arrayIndex);
            }
        }

        /// <summary>
        /// Locks and then removes the item from the containing collection
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            lock (lockObject)
            {
                return list.Remove(item);
            }
        }

        /// <summary>
        /// Thread locks, and returns the lenght of the internal list.
        /// </summary>
        public int Count
        {
            get { return list.Count; }
        }

        /// <summary>
        /// The list isn't readonly.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Locks and the calls the internal lists indexOf method.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(T item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            lock (lockObject)
            {
                list.Insert(index, item);
            }
        }

        public void RemoveAt(int index)
        {
            lock (lockObject)
            {
                list.RemoveAt(index);
            }
        }

        public T this[int index]
        {
            get { return list[index]; }
            set
            {
                lock (lockObject)
                {
                    list[index] = value;
                }
            }
        }
    }
}