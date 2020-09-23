namespace _03.MinHeap
{
    using System;
    using System.Collections.Generic;

    public class MinHeap<T> : IAbstractHeap<T>
        where T : IComparable<T>
    {
        private List<T> _elements;

        public MinHeap()
        {
            this._elements = new List<T>();
        }

        public int Size => this._elements.Count;

        public T Dequeue()
        {
            this.EnsureNotEmpty();

            var firstElement = this.Peek();
            this.Swap(0, this.Size - 1);
            this._elements.RemoveAt(this.Size - 1);

            this.HeapifyDown();

            return firstElement;
        }

        public void Add(T element)
        {
            this._elements.Add(element);

            this.HeapifyUp();
        }

        public T Peek()
        {
            this.EnsureNotEmpty();
            return this._elements[0];
        }

        private void HeapifyUp()
        {
            int currentIndex = this.Size - 1;
            int parentIndex = this.GetParentIndex(currentIndex);
            while (this.IndexIsValid(currentIndex) && this.IsLess(currentIndex, parentIndex))
            {
                this.Swap(currentIndex, parentIndex);
                currentIndex = parentIndex;
                parentIndex = this.GetParentIndex(currentIndex);
            }
        }

        private void HeapifyDown()
        {
            int index = 0;
            int swapIndex = this.GetLeftChildIndex(index);

            while (swapIndex < this._elements.Count)
            {
                int rightChildIndex  = this.GetRightChildIndex(index);
                
                if (rightChildIndex < this._elements.Count)
                {
                    swapIndex = IsLess(swapIndex, rightChildIndex) ? swapIndex : rightChildIndex;
                }
                if (IsLess(index, swapIndex))
                {
                    break;
                }

                this.Swap(index, swapIndex);

                index = swapIndex;
                swapIndex = GetLeftChildIndex(index);
            }
        }

        private void Swap(int currentIndex, int parentIndex)
        {
            var temp = this._elements[currentIndex];
            this._elements[currentIndex] = this._elements[parentIndex];
            this._elements[parentIndex] = temp;

        }

        private bool IsLess(int childIndex, int parentIndex)
        {
            return this._elements[childIndex].CompareTo(this._elements[parentIndex]) < 0;
        }

        private int GetParentIndex(int childIndex)
        {
            return (childIndex - 1) / 2;
        }

        private int GetLeftChildIndex(int parentIndex)
        {
            return 2 * parentIndex + 1;
        }

        private int GetRightChildIndex(int parentIndex)
        {
            return 2 * parentIndex + 2;
        }

        private bool IndexIsValid(int index)
        {
            return index > 0 && index < this.Size;
        }

        private void EnsureNotEmpty()
        {
            if (this.Size == 0)
            {
                throw new InvalidOperationException("Max heap is empty");
            }
        }
    }
}