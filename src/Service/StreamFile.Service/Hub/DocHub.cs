using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;

namespace StreamFile.Service.Hub
{
    public interface IDocHub
    {

    }
    public class DocHub : Hub<IDocHub>
    {

    }
}
