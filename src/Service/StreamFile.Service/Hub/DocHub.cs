using Microsoft.AspNetCore.SignalR;
using Serilog;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StreamFile.Service.Hub
{
    public interface IDocHub
    {
        Task LinkDoc(string name ,string message);
    }
    public class DocHub : Hub<IDocHub>
    {
        private readonly ILogger _logger;
        public DocHub()
        {
            _logger = Log.Logger;
        }
        public Task SendMessageToUser(string name, string message)
        {

            return Clients.All.LinkDoc(name, message);
        }

        public override Task OnConnectedAsync()
        {
            var name = GetUsername();
            _logger.Information("Start Connect");
            _logger.Information($"ConnectionId: {Context.ConnectionId}");
            _logger.Information($"Name: {name}");

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception e)
        {
            var name = GetUsername();
            _logger.Information("End Connect");
            _logger.Information($"ConnectionId: {Context.ConnectionId}");
            _logger.Information($"Name: {name}");
            return base.OnDisconnectedAsync(e);
        }

        private string GetUsername()
        {
            var name = Context.User?.Identity?.Name?.ToLowerInvariant();
            if (string.IsNullOrEmpty(name))
            {
                name = Context.User.FindFirstValue("name");
            }

            return string.IsNullOrEmpty(name) ? Context.ConnectionId : name.ToLowerInvariant();
        }
    }
}
