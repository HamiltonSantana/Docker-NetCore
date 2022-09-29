using Microsoft.AspNetCore.SignalR;
using ServerSide.Models;

namespace ServerSide
{
    public class SignalHub : Hub
    {
        public void NotificationMessage(string message)
        {
            Clients.All.SendAsync("NotificationMessage", message);
        }

        public void SendNotification(string msg)
        {
            Clients.All.SendAsync("NotificationMessage", msg);
        }
    }
}
