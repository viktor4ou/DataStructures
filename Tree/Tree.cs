using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Linq;

namespace Tree
{
    using System;
    using System.Collections.Generic;

    public class Tree<T> : IAbstractTree<T>
    {

        public Tree(T value)
        {
            Value = value;
            Children = new List<Tree<T>>();
        }

        public Tree(T value, params Tree<T>[] children)
            : this(value)
        {
            Value = value;
            Children = new List<Tree<T>>();
            foreach (Tree<T> child in children)
            {
                child.Parent = this;
                Children.Add(child);
            }
        }
        private T Value { get; set; }
        private List<Tree<T>> Children { get; set; }
        private Tree<T> Parent { get; set; }

        public void AddChild(T parentValue, Tree<T> child)
        {
            Tree<T> searchedTree = FindTreeDFS(parentValue,this);
            if (searchedTree is null)
            {
                throw new ArgumentNullException();
            }
            searchedTree.Children.Add(child);
        }

        private Tree<T> FindTreeBFS(T parentValue)
        {
            Queue<Tree<T>> queue = new Queue<Tree<T>>();
            queue.Enqueue(this);

            while (queue.Count != 0)
            {
                Tree<T> currentTree = queue.Dequeue();

                if (currentTree.Value.Equals(parentValue))
                {
                    return currentTree;
                }

                foreach (Tree<T> child in currentTree.Children)
                {
                    queue.Enqueue(child);
                }
            }

            return null;
        }

        //Check how to find Tree with DFS , is there a better way, it doesn't work for the root
        private Tree<T> FindTreeDFS(T parentValue,Tree<T> tree)
        {
            if (tree is null)
            {
                return null;
            }
            if (tree.Value.Equals(parentValue))
            {
                return tree;
            }
            foreach (Tree<T> child in tree.Children)
            {
                Tree<T> resultTree = FindTreeDFS(parentValue, child);
                if (resultTree != null )
                {
                    return resultTree;
                }
            }

            return null;
        }

        public IEnumerable<T> OrderBfs()
        {
            Queue<Tree<T>> queue = new Queue<Tree<T>>();
            List<T> list = new List<T>();
            queue.Enqueue(this);
            while (queue.Count != 0)
            {
                Tree<T> currentTree = queue.Dequeue();

                foreach (Tree<T> child in currentTree.Children)
                {
                    queue.Enqueue(child);
                }

                list.Add(currentTree.Value);
            }
            return list;
        }

        public IEnumerable<T> OrderDfs()
        {
            List<T> list = new List<T>();
            OrderDfs(list, this);

            return list;
        }

        private void OrderDfs(List<T> list, Tree<T> tree)
        {
            if (tree is null)
            {
                return;
            }

            foreach (Tree<T> child in tree.Children)
            {
                OrderDfs(list, child);
            }
            list.Add(tree.Value);
        }
        //Find the node with DFS, test it
        public void RemoveNode(T nodeKey)
        {
            Tree<T> searchedTree = FindTreeDFS(nodeKey,this);
            if (searchedTree is null)
            {
                throw new ArgumentNullException();
            }
            else if (searchedTree.Parent is null)
            {
                throw new ArgumentException();
            }

            RemoveTreeChildren(searchedTree);
            Tree<T> parentTree = searchedTree.Parent;
            parentTree.Children.Remove(searchedTree);
        }

        private void RemoveTreeChildren(Tree<T> searchedTree)
        {
            for (int i = 0; i < searchedTree.Children.Count; i++)
            {
                searchedTree.Children.RemoveAt(i );
            }
        }

        public void Swap(T firstKey, T secondKey)
        {
            
            Tree<T> firstTree = FindTreeBFS(firstKey);
            Tree<T> secondTree = FindTreeBFS(secondKey);
            if (firstTree is null || secondTree is null)
            {
                throw new ArgumentNullException();
            }
            if (firstTree.Parent is null || secondTree.Parent is null)
            {
                throw new ArgumentException();
            }

            Tree<T> firstTreeParent = firstTree.Parent;
            Tree<T> secondTreeParent = secondTree.Parent;

            int firstTreeIndex = firstTreeParent.Children.IndexOf(firstTree);
            int secondTreeIndex = secondTreeParent.Children.IndexOf(secondTree);

            firstTreeParent.Children[firstTreeIndex] = secondTree;
            secondTreeParent.Children[secondTreeIndex] = firstTree;
        }
    }
}
