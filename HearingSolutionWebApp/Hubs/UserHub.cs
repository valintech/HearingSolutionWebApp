using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.Identity;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using HearingSolutionWebApp.Models;

namespace HearingSolutionWebApp.Hubs
{
    public class UserHub : Hub
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

        #region properties for repetitve checks
        private bool UserIsAutenticated { get { return Context != null && Context.User != null && Context.User.Identity != null && Context.User.Identity.IsAuthenticated; } }

        private string currentUserId { get { if (_currentUserId != null) return _currentUserId; else { _currentUserId = Context.User.Identity.GetUserId(); return _currentUserId; } } }
        private string _currentUserId;

        private AspNetUser currentUser { get { return db.AspNetUsers.FirstOrDefault(x => x.Id == currentUserId); } }

        private bool isAdministrator { get { return currentUser != null && currentUser.IsAdministrator.HasValue && currentUser.IsAdministrator.Value; } }
        #endregion

        public void getAllUsers()
        {
            if (UserIsAutenticated)// authentication check
            {
                Models.UserVM[] data = new Models.UserVM[0];
                using (db = new Entities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    if (currentUser != null)
                    {
                        if (isAdministrator)
                            data = db.AspNetUsers.Where(x => x.DateDeleted == null).Select(a => new Models.UserVM//Casting dbKiosk from to kioskVM
                            {
                                Ids = a.Id,
                                UserName = a.UserName,
                                CompanyName = a.CompanyName
                            }).ToArray();

                    }
                    Clients.Caller.allUsersData(data);
                }
            }

        }
    }
}