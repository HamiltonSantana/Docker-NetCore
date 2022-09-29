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

        public void SendNotification(User usr)
        {
            Clients.All.SendAsync("SendNotification", usr);
        }
    }
}
