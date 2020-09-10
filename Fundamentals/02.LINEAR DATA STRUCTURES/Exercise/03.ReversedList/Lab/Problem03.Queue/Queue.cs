namespace Problem03.Queue
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;

    public class Queue<T> : IAbstractQueue<T>
    {
        private Node<T> _head;

        public int Count { get; private set; }

        public Queue()
        {
            this._head = null;
            this.Count = 0;
        }

        public Queue(Node<T> head)
        {
            this._head = head;
            this.Count = 1;
        }

        public bool Contains(T item)
        {
            this.ValidateIfNotEmpty();
            var current = this._head;

            while (current != null)
            {
                if (current.Value.Equals(item))
                {
                    return true;
                }
                current = current.Next;
            }

            return false;
        }

        public T Dequeue()
        {
            this.ValidateIfNotEmpty();
            var current = this._head;
            this._head = current.Next;
            this.Count--;
            return current.Value;
        }

        public void Enqueue(T item)
        {
            var current = this._head;
            var toInsert = new Node<T>(item);

            if (current == null)
            {
                this._head = toInsert;
            }
            else
            {
                while (current.Next != null)
                {
                    current = current.Next;
                }

                current.Next = toInsert;
            }
            this.Count++;
        }

        
        public T Peek()
        {
            this.ValidateIfNotEmpty();
            
            return this._head.Value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var current = this._head;

            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();

        private void ValidateIfNotEmpty()
        {
            if (this.Count <= 0)
            {
                throw new InvalidOperationException("The queue is empty!");
            }
        }
    }
}