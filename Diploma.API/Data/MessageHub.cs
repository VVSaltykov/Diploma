using Diploma.Common.Models;
using Microsoft.AspNetCore.SignalR;

namespace Diploma.API.Data;

public class MessageHub : Hub
{
    public async Task SendMessage(Messages message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }
}