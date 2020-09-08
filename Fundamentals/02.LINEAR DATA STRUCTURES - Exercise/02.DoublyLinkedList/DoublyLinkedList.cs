namespace Problem02.DoublyLinkedList
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Transactions;

    public class DoublyLinkedList<T> : IAbstractLinkedList<T>
    {
        private Node<T> head;
        private Node<T> tail;

        public int Count { get; private set; }

        public void AddFirst(T item)
        {
            var newNode = new Node<T>
            {
                Item = item,
                Next = this.head
            };

            if(this.head != null)
            {
                this.head.Previous = newNode;
            }
            this.head = newNode;

            if (this.head.Next != null && this.tail is null)
            {
                var current = this.head;

                while (current.Next != null)
                    current = current.Next;

                this.tail = current;
            }
            
            this.Count++;
        }

        public void AddLast(T item)
        {
            var newNode = new Node<T>
            {
                Item = item,
                Previous = this.tail
            };

            if (this.head is null)
                this.head = newNode;
            else
            {
                if (this.tail == null)
                {
                    this.tail = newNode;
                    this.head.Next = newNode;
                }
                else
                {
                    //var lastNode = this._tail;
                    this.tail.Next = newNode;
                    this.tail = newNode;
                }
            }

            this.Count++;
        }

        public T GetFirst()
        {
            this.EnsureNotEmpty();

            return this.head.Item;
        }

        public T GetLast()
        {
            this.EnsureNotEmpty();

            return this.tail.Item;
        }

        public T RemoveFirst()
        {
            this.EnsureNotEmpty();

            var headItem = this.head.Item;
            var newHead = this.head.Next;
            this.head.Next = null;
            this.head = newHead;
            this.Count--;

            return headItem;
        }

        public T RemoveLast()
        {
            this.EnsureNotEmpty();

            if (this.head.Next is null)
                return this.RemoveFirst();


            var lastItem = this.tail.Item;
            this.tail.Next = null;
            this.tail = this.tail.Previous;
            this.Count--;

            return lastItem;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var current = this.head;

            while (current != null)
            {
                yield return current.Item;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        private void EnsureNotEmpty()
        {
            if (this.Count == 0)
                throw new InvalidOperationException();
        }
    }
}