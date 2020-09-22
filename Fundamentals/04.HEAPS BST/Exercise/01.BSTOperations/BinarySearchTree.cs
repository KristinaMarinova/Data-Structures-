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
            this.EachInOrder(this.Root, action);
        }     
        public List<T> Range(T lower, T upper)
        {
            List<T> queue = new List<T>();

            this.Range(this.Root, queue, lower, upper);

            return queue;
        }
        public void DeleteMin()
        {
            if (this.Root == null)
            {
                throw new InvalidOperationException();
            };
            this.Root = deleteMin(this.Root);
        }
        public void DeleteMax()
        {
            if (this.Root == null)
            {
                throw new InvalidOperationException();
            };
            this.Root = deleteMax(this.Root);
        }
        public int GetRank(T element)
        {
            if (this.Root == null)
            {
                return 0;
            }
            var result = new List<T>();
            Rank(result, element, this.Root);

            return result.Count;
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
        private void EachInOrder(Node<T> node, Action<T> action)
        {
            if (node == null)
            {
                return;
            }

            this.EachInOrder(node.LeftChild, action);
            action(node.Value);
            this.EachInOrder(node.RightChild, action);
        }
        private void Range(Node<T> node, List<T> list, T startRange, T endRange)
        {
            if (node == null)
            {
                return;
            }

            int nodeInLowerRange = startRange.CompareTo(node.Value);
            int nodeInHigherRange = endRange.CompareTo(node.Value);

            if (nodeInLowerRange < 0)
            {
                this.Range(node.LeftChild, list, startRange, endRange);
            }
            if (nodeInLowerRange <= 0 && nodeInHigherRange >= 0)
            {
                list.Add(node.Value);
            }
            if (nodeInHigherRange > 0)
            {
                this.Range(node.RightChild, list, startRange, endRange);
            }
        }
        private Node<T> deleteMin(Node<T> n)
        {
            if (n.LeftChild == null)
                return n.RightChild;
            n.LeftChild = deleteMin(n.LeftChild);
            return n;
        }
        private Node<T> deleteMax(Node<T> n)
        {
            if (n.RightChild == null)
                return n.LeftChild;
            n.RightChild = deleteMax(n.RightChild);
            return n;
        }
        private void Rank(List<T> result, T element, Node<T> node)
        {
            if (node == null)
            {
                return;
            }
            if (node.Value.CompareTo(element) < 0)
            {
                result.Add(node.Value);
            }
            Rank(result, element, node.LeftChild);
            Rank(result, element, node.RightChild);
        }
    }
}
