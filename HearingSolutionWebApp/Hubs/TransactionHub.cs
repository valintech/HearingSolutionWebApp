using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.AspNet.Identity;

namespace HearingSolutionWebApp.Hubs
{
    public class TransactionHub : Hub
    {
        public static ConcurrentDictionary<string, ConnectedUser> ConnectedUsers = new ConcurrentDictionary<string, ConnectedUser>(); // stores connected users signalrRId tied with db userId

        public override Task OnConnected()
        {
            ConnectedUsers.TryAdd(Context.ConnectionId, new ConnectedUser() { ConnectionId = Context.ConnectionId });
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            ConnectedUser garbage;
            ConnectedUsers.TryRemove(Context.ConnectionId, out garbage);
            return base.OnDisconnected(stopCalled);
        }

        private Entities db;

        #region properties for repetitves
        private bool UserIsAutenticated { get { return Context != null && Context.User != null && Context.User.Identity != null && Context.User.Identity.IsAuthenticated; } }

        private string currentUserId { get { if (_currentUserId != null) return _currentUserId; else { _currentUserId = Context.User.Identity.GetUserId(); return _currentUserId; } } }
        private string _currentUserId;

        private AspNetUser currentUser { get { return db.AspNetUsers.FirstOrDefault(x => x.Id == currentUserId); } }

        private bool isAdministrator { get { return currentUser != null && currentUser.IsAdministrator.HasValue && currentUser.IsAdministrator.Value; } }

        #endregion
    }
}