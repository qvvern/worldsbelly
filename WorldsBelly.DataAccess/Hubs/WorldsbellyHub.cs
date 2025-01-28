using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldsBelly.DataAccess.Hubs
{
    public class WorldsBellyHub : Hub
    {
        public Task SubscribeToNotifications(string userGuid)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, userGuid);
        }
        public Task UnsubscribeToNotifications(string userGuid)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, userGuid);
        }
    }
}
