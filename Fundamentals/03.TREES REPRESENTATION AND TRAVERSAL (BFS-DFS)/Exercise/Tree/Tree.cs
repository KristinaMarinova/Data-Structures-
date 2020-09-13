namespace Tree
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Tree<T> : IAbstractTree<T>
    {
        private readonly List<Tree<T>> _children;

        public Tree(T key)
        {
            this.Key = key;
            this.Parent = null;
            this._children = new List<Tree<T>>();
        }

        public Tree(T key, params Tree<T>[] children) : this(key)
        {
            foreach (var child in children)
            {
                child.Parent = this;
                this._children.Add(child);
            }
        }

        public T Key { get; private set; }

        public Tree<T> Parent { get; private set; }


        public IReadOnlyCollection<Tree<T>> Children
            => this._children.AsReadOnly();

        public void AddChild(Tree<T> child)
        {
            this._children.Add(child);
        }

        public void AddParent(Tree<T> parent)
        {
            this.Parent = parent;
        }

        public string GetAsString()
        {
            var sb = new StringBuilder();
            PrintTree(this, sb, 0);
            return sb.ToString().Trim();
        }

        private void PrintTree(Tree<T> tree, StringBuilder sb, int indent = 0)
        {
            if (tree == null)
            {
                return;
            }

            sb = sb.AppendLine($"{new string(' ', indent)}{tree.Key}");

            foreach (var child in tree.Children)
            {
                PrintTree(child, sb, indent + 2);
            }
        }

        public Tree<T> GetDeepestLeftomostNode()
        {
            Tree<T> res = this;

            Queue<Tree<T>> nodes = new Queue<Tree<T>>();
            nodes.Enqueue(res);

            while (nodes.Count > 0)
            {
                res = nodes.Dequeue();
                if (res._children.Count == 0)
                {
                    return res;
                }

                nodes.Enqueue(res._children[0]);
            }

            return res;
        }

        public List<T> GetLeafKeys()
        {
            var result = new List<T>();
            var queue = new Queue<Tree<T>>();

            queue.Enqueue(this);

            while (queue.Count != 0)
            {
                var subtree = queue.Dequeue();

                if(subtree._children.Count == 0)
                {
                    result.Add(subtree.Key);
                }

                foreach (var child in subtree.Children)
                {
                    queue.Enqueue(child);
                }
            }

            return result;
        }

        public List<T> GetMiddleKeys()
        {
            var result = new List<T>();
            var queue = new Queue<Tree<T>>();

            queue.Enqueue(this);

            while (queue.Count != 0)
            {
                var subtree = queue.Dequeue();

                if (subtree._children.Count > 0 && subtree.Parent != null)
                {
                    result.Add(subtree.Key);
                }

                foreach (var child in subtree.Children)
                {
                    queue.Enqueue(child);
                }
            }
            result = result.OrderBy(x => x).ToList();
            return result;
        }

        public List<T> GetLongestPath()
        {
            List<T> path = FindLongestPath(this);
            return path;
        }

        private List<T> FindLongestPath(Tree<T> tree)
        {
            var result = new List<T>();
            List<T> path;

            foreach (var child in tree.Children)
            {
                path = FindLongestPath(child);

                if (path.Count > result.Count)
                {
                    result = path;
                }
            }

            result.Insert(0, tree.Key);
            return result;
        }

        public List<List<T>> PathsWithGivenSum(int sum)
        {
            var result = new List<List<T>>();
            PathsWithGivenSum(this, 0, sum, result);
            return result;
        }

        public void PathsWithGivenSum(Tree<T> subtree, int sum, int targetSum, List<List<T>> result)
        {
            sum += Convert.ToInt32(subtree.Key);

            if (sum == targetSum)
            {
                var path = new Stack<T>();
                var current = subtree;
                path.Push(current.Key);
                while (current.Parent != null)
                {
                    current = current.Parent;
                    path.Push(current.Key);
                }

                var resultPath = path.ToList();
                result.Add(resultPath);
            }

            foreach (var child in subtree.Children)
            {
                PathsWithGivenSum(child, sum, targetSum, result);
            }
        }

        public List<Tree<T>> SubTreesWithGivenSum(int sum)
        {
            List<Tree<T>> subtrees = new List<Tree<T>>();
            GetSubTreesWithGivenSum(this, sum, 0, subtrees);
            return subtrees;
        }

        private int GetSubTreesWithGivenSum(Tree<T> tree, int targetSum, int sum, List<Tree<T>> subtrees)
        {
            sum = Convert.ToInt32(tree.Key);

            foreach (var child in tree.Children)
            {
                sum += GetSubTreesWithGivenSum(child, targetSum, sum, subtrees);
            }

            if (sum == targetSum)
            {
                subtrees.Add(tree);
            }

            return sum;
        }
    }
}