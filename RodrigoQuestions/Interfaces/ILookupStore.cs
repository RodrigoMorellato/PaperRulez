using System.Collections.Generic;

namespace RodrigoQuestions.Interfaces
{
    public interface ILookupStore
    {
        void Record(string client, string documentId, IEnumerable<string> keywords);
    }
}
