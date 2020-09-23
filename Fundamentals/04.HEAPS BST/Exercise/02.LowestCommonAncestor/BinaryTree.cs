namespace _02.LowestCommonAncestor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class BinaryTree<T> : IAbstractBinaryTree<T>
        where T : IComparable<T>
    {
        public BinaryTree(
            T value,
            BinaryTree<T> leftChild,
            BinaryTree<T> rightChild)
        {
            this.Value = value;
            this.LeftChild = leftChild;
            this.RightChild = rightChild;
        }

        public T Value { get; set; }

        public BinaryTree<T> LeftChild { get; set; }

        public BinaryTree<T> RightChild { get; set; }

        public BinaryTree<T> Parent { get; set; }

        public T FindLowestCommonAncestor(T first, T second)
        {
            List<T> firstPath = findPath(first);
            List<T> secondPath = findPath(second);

            int smallerSize = firstPath.Count() < secondPath.Count() ? firstPath.Count() : secondPath.Count();

            var i = 0;
            for (; i < smallerSize; i++)
            {
                if (!firstPath[i].Equals(secondPath[i]))
                {
                    break;
                }
            }

            return firstPath[i - 1];
        }

        private List<T> findPath(T element)
        {
            List<T> result = new List<T>();
            findNodePath(this, element, result);

            return result;
        }

        private bool findNodePath(BinaryTree<T> binaryTree, T element, List<T> currentPath)
        {
            if (binaryTree == null) { return false; }
            if (binaryTree.Value.Equals(element)) { return true; }

            currentPath.Add(binaryTree.Value);

            bool leftResult = findNodePath(binaryTree.LeftChild, element, currentPath);
            if (leftResult) { return true; }

            bool rightResult = findNodePath(binaryTree.RightChild, element, currentPath);
            if (rightResult) { return true; }


            currentPath.Remove(binaryTree.Value); // value of

            return false;
        }
    }
}
