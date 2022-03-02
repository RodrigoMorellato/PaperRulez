using System.Collections.Generic;
using System.Threading.Tasks;

namespace RodrigoQuestions.Interfaces
{
    public interface IPaperRulez
    {
        Task ProcessClients(IList<string> clients);
        Task ProcessClient(string client);
    }
}
