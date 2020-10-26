namespace _01.Hierarchy
{
    using System;
    using System.Collections.Generic;
    using System.Collections;

    public class Hierarchy<T> : IHierarchy<T>
    {
        private Node<T> root;
        private Dictionary<T, Node<T>> elements = new Dictionary<T, Node<T>>();
        public Hierarchy(T root)
        {
            this.root = CreateNode(root);
        }

        public int Count { get => elements.Count; }

        public void Add(T element, T child)
        {
            ContainsItemOrThrowExeption(element);

            if (elements.ContainsKey(child))
            {
                throw new ArgumentException();
            }
            var node = CreateNode(child);
            node.Parent = elements[element];
            elements[element].Children.Add(node);
        }

        public void Remove(T element)
        {
            if (this.root.Value.Equals(element))
            {
                throw new InvalidOperationException();
            }

            ContainsItemOrThrowExeption(element);

            DestroyElement(element);
        }
        public IEnumerable<T> GetChildren(T element)
        {
            throw new NotImplementedException();
        }

        public T GetParent(T element)
        {
            throw new NotImplementedException();
        }

        public bool Contains(T element)
        {
            return elements.ContainsKey(element);
        }

        public IEnumerable<T> GetCommonElements(Hierarchy<T> other)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void ContainsItemOrThrowExeption(T element)
        {
            if (!Contains(element))
            {
                throw new ArgumentException();
            }
        }
        private Node<T> CreateNode(T element)
        {
            var node = new Node<T>(element);
            elements[element] = node;
            return node;
        }
        private void DestroyElement(T element)
        {
            var node = elements[element];
            node.Parent?.Children.Remove(node);
            if (node.Parent != null && node.Children.Count > 0)
            {
                foreach (var child in node.Children)
                {
                    child.Parent = node.Parent;
                    node.Parent.Children.Add(child);
                }
            }
            elements.Remove(element);
        }

    }
}