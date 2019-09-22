using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace NoobsGameOfLife.Core
{
    public class ConcurrentCollection<T> : ICollection<T>
    {
        private readonly List<T> internalCollection;
        private readonly SemaphoreSlim semaphoreSlim;

        public ConcurrentCollection()
        {
            internalCollection = new List<T>();
            semaphoreSlim = new SemaphoreSlim(1, 1);
        }

        public int Count => internalCollection.Count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            semaphoreSlim.Wait();
            internalCollection.Add(item);
            semaphoreSlim.Release();
        }

        public void AddRange(IEnumerable<T> items)
        {
            semaphoreSlim.Wait();
            internalCollection.AddRange(items);
            semaphoreSlim.Release();
        }

        public void Clear()
        {
            semaphoreSlim.Wait();
            internalCollection.Clear();
            semaphoreSlim.Release();
        }

        public bool Contains(T item)
        {
            semaphoreSlim.Wait();
            var result = internalCollection.Contains(item);
            semaphoreSlim.Release();
            return result;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            semaphoreSlim.Wait();
            Buffer.BlockCopy(internalCollection.ToArray(), 0, array, arrayIndex, internalCollection.Count);
            semaphoreSlim.Release();
        }

        public bool Remove(T item)
        {
            semaphoreSlim.Wait();
            var result = internalCollection.Remove(item);
            semaphoreSlim.Release();
            return result;
        }

        public T[] ToArray()
        {
            semaphoreSlim.Wait();
            var array = internalCollection.ToArray();
            semaphoreSlim.Release();
            return array;
        }

        public IEnumerator<T> GetEnumerator() => new Enumerator(this);
        
        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        private struct Enumerator : IEnumerator, IEnumerator<T>
        {
            public object Current => array[index];

            T IEnumerator<T>.Current => array[index];

            private readonly T[] array;
            private int index;

            public Enumerator(ConcurrentCollection<T> collection)
            {
                array = collection.ToArray();
                index = -1;
            }

            public bool MoveNext()
            {
                index++;

                return index >= 0 && index < array.Length;
            }

            public void Reset()
            {
                index = -1;
            }

            public void Dispose() { }
        }        
    }
}
