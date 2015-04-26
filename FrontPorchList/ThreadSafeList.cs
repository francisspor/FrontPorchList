using System.Collections;
using System.Collections.Generic;

namespace FrontPorchList
{
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
            lock (lockObject)
            {
                return list.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (lockObject)
            {
                return ((IList) list).GetEnumerator();
            }
        }

        public void Add(T item)
        {
            lock (lockObject)
            {
                list.Add(item);
            }
        }

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
            lock (lockObject)
            {
                return list.Contains(item);
            }
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
            get
            {
                lock (lockObject)
                {
                    return list.Count;
                }
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public int IndexOf(T item)
        {
            lock (lockObject)
            {
                return list.IndexOf(item);
            }
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
            get
            {
                lock (lockObject)
                {
                    return list[index];
                }
            }
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