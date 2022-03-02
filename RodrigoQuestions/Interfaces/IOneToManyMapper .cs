using System.Collections.Generic;

namespace RodrigoQuestions.Interfaces
{
    interface IOneToManyMapper
    {
        void Add(int parent, int child);
        void RemoveParent(int parent);
        void RemoveChild(int child);
        IEnumerable<int> GetChildren(int parent);
        int GetParent(int child);
        void UpdateParent(int oldParent, int newParent);
        void UpdateChild(int oldChild, int newChild);
    }
}
