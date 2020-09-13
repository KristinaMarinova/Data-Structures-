namespace Tree
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TreeFactory
    {
        private Dictionary<int, Tree<int>> nodesBykeys;

        public TreeFactory()
        {
            this.nodesBykeys = new Dictionary<int, Tree<int>>();
        }

        public Tree<int> CreateTreeFromStrings(string[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                var tokens = input[i].Split(' ').Select(int.Parse).ToArray();
                var parentInt = tokens[0];
                var childInt = tokens[1];

                this.AddEdge(parentInt, childInt);
            }

            return this.GetRoot();
        }

        public Tree<int> CreateNodeByKey(int key)
        {
            return new Tree<int>(key);
        }

        public void AddEdge(int parent, int child)
        {
            var childTree = this.CreateNodeByKey(child);
            if (this.nodesBykeys.ContainsKey(parent))
            {
                this.nodesBykeys[parent].AddChild(childTree);
            }
            else
            {
                var parentTree = this.CreateNodeByKey(parent);
                this.nodesBykeys.Add(parent, parentTree);
                this.nodesBykeys[parent].AddChild(childTree);
            }

            childTree.AddParent(this.nodesBykeys[parent]);
            this.nodesBykeys.Add(child, childTree);
        }

        private Tree<int> GetRoot()
        {
            var kvp = this.nodesBykeys.FirstOrDefault();
            return kvp.Value;
        }
    }
}