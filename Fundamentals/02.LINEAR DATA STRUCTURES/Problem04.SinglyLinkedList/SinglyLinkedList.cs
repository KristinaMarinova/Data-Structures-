namespace Problem04.SinglyLinkedList
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class SinglyLinkedList<T> : IAbstractLinkedList<T>
    {
        private Node<T> _head;

        public int Count { get; private set; }

        public SinglyLinkedList()
        {
            this._head = null;
            this.Count = 0;
        }

        public SinglyLinkedList(Node<T> head)
        {
            this._head = head;
            this.Count = 1;
        }

        public void AddFirst(T item)
        {
            var toInsert = new Node<T>(item, this._head);
            this._head = toInsert;
            this.Count++;
        }

        public void AddLast(T item)
        {
            var toInsert = new Node<T>(item);
            var current = this._head;

            if(current == null)
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

        public T GetFirst()
        {
            this.ValidateIfNotEmpty();
            return this._head.Value;
        }

        public T GetLast()
        {
            this.ValidateIfNotEmpty();
            var current = this._head;

            while (current.Next != null)
            {
                current = current.Next;
            }

            return current.Value;
        }

        public T RemoveFirst()
        {
            this.ValidateIfNotEmpty();
            var toRemove = this._head;
            this._head = this._head.Next;
            this.Count--;
            return toRemove.Value;
        }

        public T RemoveLast()
        {
            this.ValidateIfNotEmpty();

            if (this.Count == 1)
            {
                var toReturn = this._head.Value;
                this._head = null;
                this.Count--;
                return toReturn;
            }
            else
            {
                var current = this._head;

                while (current.Next.Next != null)
                {
                    current = current.Next;
                }

                var toReturn = current.Next;
                current.Next = null;
                this.Count--;
                return toReturn.Value;
                
            }
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
                throw new InvalidOperationException("The LikendList is empty!");
            }
        }
    }
}