namespace _01.BSTOperations
{
    using System;
    using System.Collections.Generic;

    public class BinarySearchTree<T> : IAbstractBinarySearchTree<T>
        where T : IComparable<T>
    {
        public BinarySearchTree()
        {
        }

        public BinarySearchTree(Node<T> root)
        {
            this.Copy(root);
        }

       
        public Node<T> Root { get; private set; }

        public Node<T> LeftChild { get; private set; }

        public Node<T> RightChild { get; private set; }

        public T Value => this.Root.Value;

        public int Count => this.ElementsCount();


        public bool Contains(T element)
        {
            var current = this.Root;

            while (current != null)
            {
                if (this.IsLess(element, current.Value))
                {
                    current = current.LeftChild;
                }
                else if (this.IsGreater(element, current.Value))
                {
                    current = current.RightChild;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        public void Insert(T element)
        {
            var toInsert = new Node<T>(element, null, null);

            if (this.Root == null)
            {
                this.Root = toInsert;
            }
            else
            {
                this.InsertRecursive(this.Root, element);
            }
        }

        public IAbstractBinarySearchTree<T> Search(T element)
        {
            var current = this.Root;

            while (current != null && !this.AreEqual(element, current.Value))
            {
                if (this.IsLess(element, current.Value))
                {
                    current = current.LeftChild;
                }
                else if (this.IsGreater(element, current.Value))
                {
                    current = current.RightChild;
                }
            }

            return new BinarySearchTree<T>(current);
        }

        public void EachInOrder(Action<T> action)
        {
            throw new NotImplementedException();
        }

        public List<T> Range(T lower, T upper)
        {
            throw new NotImplementedException();
        }

        public void DeleteMin()
        {
            throw new NotImplementedException();
        }

        public void DeleteMax()
        {
            throw new NotImplementedException();
        }

        public int GetRank(T element)
        {
            throw new NotImplementedException();
        }



        private void Copy(Node<T> root)
        {
            if (root != null)
            {
                this.Insert(root.Value);
                this.Copy(root.LeftChild);
                this.Copy(root.RightChild);
            }
        }

        private bool IsLess(T element, T value)
        {
            return element.CompareTo(value) < 0;
        }

        private bool IsGreater(T element, T value)
        {
            return element.CompareTo(value) > 0;
        }

        private bool AreEqual(T element, T value)
        {
            return element.CompareTo(value) == 0;
        }

        private int ElementsCount()
        {
            if (this.Root == null)
            {
                return 0;
            }

            var count = 0;

            this.CountOrderDfs(this.Root, ref count);

            return count;
        }

        private void CountOrderDfs(Node<T> subtree, ref int count)
        {
            var current = subtree;

            if (current == null)
            {
                return;
            }

            count++;
            CountOrderDfs(current.LeftChild, ref count);
            CountOrderDfs(current.RightChild, ref count);
        }

        private Node<T> InsertRecursive(Node<T> current, T element)
        {
            var toInsert = new Node<T>(element, null, null);
            if (current == null)
            {
                return toInsert;
            }

            if (this.IsLess(element, current.Value))
            {
                current.LeftChild = this.InsertRecursive(current.LeftChild, element);
                if (this.LeftChild == null)
                {
                    this.LeftChild = toInsert;
                }
            }
            else if (this.IsGreater(element, current.Value))
            {
                current.RightChild = this.InsertRecursive(current.RightChild, element);
                if (this.RightChild == null)
                {
                    this.RightChild = toInsert;
                }
            }

            return current;
        }
    }
}
