using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace StreamFile.Service.Hub
{
    public interface IDocHub
    {
        Task LinkDoc(object message);
    }
    public class DocHub : Hub<IDocHub>
    {
        public Task SendMessageToUser(string name, string message)
        {
            return Clients.User(name).LinkDoc(message);
        }
    }
}
