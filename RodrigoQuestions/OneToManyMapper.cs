using System;
using System.Collections.Generic;
using RodrigoQuestions.Interfaces;

namespace RodrigoQuestions
{
    public sealed class OneToManyMapper : IOneToManyMapper
    {
        // Keeps a list of all children and the respective parent for direct bottom-up search.
        private readonly Dictionary<int, int> _ancestors = new Dictionary<int, int>();

        // Keeps a list of parents and all children for direct top-down search.
        // HashSet is using to garatity best performance 
        private readonly Dictionary<int, HashSet<int>> _parents = new Dictionary<int, HashSet<int>>();

        /// <summary>
        /// Add new parent and child when both are into accepted range
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        public void Add(int parent, int child)
        {
            if (!ValidateId(parent))
                throw new ArgumentOutOfRangeException("parent");

            if (!ValidateId(child))
                throw new ArgumentOutOfRangeException("child");

            // Check if the child already has a parent.
            if (_ancestors.TryGetValue(child, out int currentParent))
            {
                if (currentParent == parent)
                    throw new ArgumentException($"Child {child} is already child of this parent.");

                throw new ArgumentException($"Child {child} already has a different parent.");
            }

            // Add the child and its parent.
            _ancestors.Add(child, parent);

            // Add the parent with child if a new parent or add the child with its siblings.
            if (!_parents.TryGetValue(parent, out HashSet<int> childrenList))
            {
                childrenList = new HashSet<int> { child };
                _parents.Add(parent, childrenList);
            }
            else
                childrenList.Add(child);
        }

        public IEnumerable<int> GetChildren(int parent)
        {
            if (!_parents.TryGetValue(parent, out HashSet<int> childrenList))
                throw new KeyNotFoundException("parent");

            return childrenList;
        }

        public int GetParent(int child)
        {
            if (!_ancestors.TryGetValue(child, out var parent))
                throw new KeyNotFoundException("child");

            return parent;
        }

        public void RemoveChild(int child)
        {
            // Get parent or throw exception if not exists.
            var parent = GetParent(child);

            _ancestors.Remove(child);

            var childList = GetChildren(parent) as HashSet<int>;
            childList.Remove(child);
            if (childList.Count == 0)
                _parents.Remove(parent); // Removes the parent if will remain no children.
        }

        public void RemoveParent(int parent)
        {
            // Get children or throw exception if not exists.
            var childList = GetChildren(parent) as HashSet<int>;

            foreach (var c in childList)
                _ancestors.Remove(c);

            _parents.Remove(parent);
        }

        public void UpdateChild(int oldChild, int newChild)
        {
            if (!ValidateId(newChild))
                throw new ArgumentOutOfRangeException("newChild");

            if (_ancestors.ContainsKey(newChild))
                throw new ArgumentException($"Child {newChild} already has a parent.");

            // Throws exception if the oldChild doesn't exists.
            var parent = GetParent(oldChild);
            var childrenList = GetChildren(parent) as HashSet<int>;

            _ancestors.Remove(oldChild);
            _ancestors.Add(newChild, parent);

            childrenList.Remove(oldChild);
            childrenList.Add(newChild);
        }

        public void UpdateParent(int oldParent, int newParent)
        {
            if (!ValidateId(newParent))
                throw new ArgumentOutOfRangeException("newParent");

            if (_parents.ContainsKey(newParent))
                throw new ArgumentException($"Parent {newParent} already exists.");

            if (!_parents.TryGetValue(oldParent, out HashSet<int> childrenList))
                throw new KeyNotFoundException("parent");

            foreach (var c in childrenList)
                _ancestors[c] = newParent;

            _parents.Remove(oldParent);
            _parents.Add(newParent, childrenList);
        }

        private static bool ValidateId(int id)
        {
            return id >= 1 && id <= 947483647;
        }
    }
}
