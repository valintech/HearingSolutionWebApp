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
    public class KioskHub : Hub
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

        private Kiosk kioskById(Guid id) { return db.Kiosks.FirstOrDefault(a => a.Id == id); }

        private Kiosk kioskById(string id) { return kioskById(new Guid(id)); }

        private bool doesUserOwnKiosk(Kiosk kiosk)
        {
            var ownership = currentUser?.KioskOwnerships.FirstOrDefault(a => a.Kiosk_Id == kiosk.Id && a.OwnershipEnd == null);
            return ownership != null;
        }

        private void BroadcastKioskUpdated(Models.KioskVM kiosk)
        {
            Clients.Caller.kioskUpdated(kiosk);//informing orginator
            foreach (var user in ConnectedUsers.Where(a => a.Value.UserInDBId != currentUserId))//selecting oppen connections exept orginator
            {
                try
                {
                    using (db = new Entities())
                    {
                        var userInDB = db.AspNetUsers.FirstOrDefault(x => x.Id == user.Value.UserInDBId);//looking for user in db
                        if (userInDB != null)
                        {
                            var ownership = userInDB.KioskOwnerships.FirstOrDefault(a => a.Kiosk_Id == kiosk.Id && a.OwnershipEnd == null);//looking for ownership
                            if (ownership != null)//if user owns the kiosk then inform him about changes
                                Clients.User(user.Key).kioskUpdated(kiosk);
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }
        }

        private void BroadcastKioskAdded(Models.KioskVM kiosk)
        {
            Clients.Caller.kioskAdded(kiosk);//informing orginator
            foreach (var user in ConnectedUsers.Where(a => a.Value.UserInDBId != currentUserId))//selecting oppen connections exept orginator
            {
                try
                {
                    using (db = new Entities())
                    {
                        var userInDB = db.AspNetUsers.FirstOrDefault(x => x.Id == user.Value.UserInDBId);//looking for user in db
                        if (userInDB != null)
                        {
                            var ownership = userInDB.KioskOwnerships.FirstOrDefault(a => a.Kiosk_Id == kiosk.Id && a.OwnershipEnd == null);//looking for ownership
                            if (ownership != null)//if user owns the kiosk then inform him about changes
                                Clients.User(user.Key).kioskAdded(kiosk);
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }
        }

        private void BroadcastKioskRemoved(Guid kioskId)
        {
            Clients.Caller.kioskRemoved(kioskId);//informing orginator
            foreach (var user in ConnectedUsers.Where(a => a.Value.UserInDBId != currentUserId))//selecting oppen connections exept orginator
            {
                try
                {
                    using (db = new Entities())
                    {
                        var userInDB = db.AspNetUsers.FirstOrDefault(x => x.Id == user.Value.UserInDBId);//looking for user in db
                        if (userInDB != null)
                        {
                            var ownership = userInDB.KioskOwnerships.FirstOrDefault(a => a.Kiosk_Id == kioskId && a.OwnershipEnd == null);//looking for ownership
                            if (ownership != null)//if user owns the kiosk then inform him about changes
                                Clients.User(user.Key).kioskRemoved(kioskId);
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }
        }
        #endregion


        public void deleteKiosk(Guid kioskId)
        {
            try
            {
                if (UserIsAutenticated)// authentication check
                {
                    using (db = new Entities())
                    {
                        db.Configuration.ProxyCreationEnabled = false;
                        if (isAdministrator)
                        {
                            var now = DateTime.Now;
                            var kiosk = kioskById(kioskId);
                            kiosk.DateDeleted = now;
                            var owned = kiosk.KioskOwnerships.FirstOrDefault(a => a.OwnershipEnd == null);
                            if (owned != null)
                                owned.OwnershipEnd = now;
                            db.SaveChanges();
                        }
                    }
                    BroadcastKioskRemoved(kioskId); 
                }
            }
            catch (Exception)
            {

            }
        }

        public void addKiosk(Models.KioskVM kiosk)
        {
            if (UserIsAutenticated)// authentication check
            {
                try
                {
                    using (db = new Entities())
                    {
                        db.Configuration.ProxyCreationEnabled = false;
                        if (isAdministrator)//only administrator can add kiosks
                        {
                            var dbKiosk = new Kiosk();

                            dbKiosk.Address = kiosk.Address;
                            dbKiosk.Email = kiosk.Email;
                            dbKiosk.Phone = kiosk.Phone;
                            dbKiosk.StoreContact = kiosk.StoreContact;
                            dbKiosk.KioskDisplayName = kiosk.KioskDisplayName;

                            db.SaveChanges(); // saved so kiosk Id will be created

                            if (!String.IsNullOrWhiteSpace(kiosk.LocationName))//location is null, and user typed someting in the field 
                            {
                                db.KioskLocations.Add(new KioskLocation() //adding new (first) location
                                {
                                    StartDate = DateTime.Now,
                                    Location = kiosk.LocationName,
                                    Kiosk_Id = dbKiosk.Id
                                });
                                db.SaveChanges();
                            }

                            
                            if (kiosk.OwnerId != null)//kiosk was created with owning user
                            {
                                var newOwner = db.AspNetUsers.FirstOrDefault(a => a.Id == kiosk.OwnerId);//finding user in db
                                if (newOwner != null)
                                {
                                    db.KioskOwnerships.Add(new KioskOwnership() { AspNetUsers_Id = newOwner.Id, Kiosk_Id = dbKiosk.Id, OwnershipStart = DateTime.Now });
                                }
                                db.SaveChanges();
                            }
                        }
                        kiosk = new Models.KioskVM(db.Kiosks.FirstOrDefault(x => x.Id == kiosk.Id));//refreshing kiosk with values from db, as it will be used to broadcast
                        BroadcastKioskAdded(kiosk);
                    }
                }
                catch (Exception e)
                {

                }
            }
        }

        public void getAllKiosks()
        {
            if (UserIsAutenticated)// authentication check
            {
                Models.KioskVM[] data = new Models.KioskVM[0];
                using (db = new Entities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    if (currentUser != null)
                    {
                        if (isAdministrator)//administrator can see all kiosks
                            data = db.Kiosks.Where(b=>b.DateDeleted==null).Select(a => new Models.KioskVM()//not using kioskVM constructor as not suported in linq to entites, make sure is essetialy same as kioskVM
                            {
                                Id = a.Id,
                                KioskDisplayName = a.KioskDisplayName,
                                StoreContact = a.StoreContact,
                                Email = a.Email,
                                Phone = a.Phone,
                                LocationName = a.KioskLocations.FirstOrDefault(b => b.EndDate == null) != null ? a.KioskLocations.FirstOrDefault(b => b.EndDate == null).Location : "",
                                CompanyName = a.KioskOwnerships.FirstOrDefault(b => b.OwnershipEnd == null) != null ? a.KioskOwnerships.FirstOrDefault(b => b.OwnershipEnd == null).AspNetUser.CompanyName : "",
                                OwnerId = a.KioskOwnerships.FirstOrDefault(b => b.OwnershipEnd == null) != null? a.KioskOwnerships.FirstOrDefault(b => b.OwnershipEnd == null).AspNetUser.Id : "",
                                DateDeleted = a.DateDeleted,
                                KioskType = a.KioskType
                            }).ToArray();
                        else //other user gets kiosks he currently owns
                            data = currentUser.KioskOwnerships.Where(c => c.Kiosk.DateDeleted == null && c.AspNetUsers_Id == currentUser.Id && c.OwnershipEnd == null)// give user kiosks he currently owns (ownerships without end date)
                            .Select(a => new Models.KioskVM()//not using kioskVM constructor as not suported in linq to entites, make sure is essetialy same as kioskVM
                            {
                                Id = a.Kiosk.Id,
                                KioskDisplayName = a.Kiosk.KioskDisplayName,
                                StoreContact = a.Kiosk.StoreContact,
                                Email = a.Kiosk.Email,
                                Phone = a.Kiosk.Phone,
                                LocationName = a.Kiosk.KioskLocations.FirstOrDefault(b => b.EndDate == null) != null ? a.Kiosk.KioskLocations.FirstOrDefault(b => b.EndDate == null).Location : "",
                                CompanyName = a.Kiosk.KioskOwnerships.FirstOrDefault(b => b.OwnershipEnd == null) != null ? a.Kiosk.KioskOwnerships.FirstOrDefault(b => b.OwnershipEnd == null).AspNetUser.CompanyName : "",
                                OwnerId = a.Kiosk.KioskOwnerships.FirstOrDefault(b => b.OwnershipEnd == null) != null ? a.Kiosk.KioskOwnerships.FirstOrDefault(b => b.OwnershipEnd == null).AspNetUser.Id : "",
                                DateDeleted = a.Kiosk.DateDeleted,
                                KioskType = a.Kiosk.KioskType
                            }).ToArray();
                    }
                }
                Clients.Caller.allKiosksData(data);
            }
        }

        /// <summary>
        /// Change of kiosk information field
        /// </summary>
        /// <param name="kiosk"></param>
        public void updateKiosk(Models.KioskVM kiosk)
        {
            if (Context != null && Context.User != null && Context.User.Identity != null && Context.User.Identity.IsAuthenticated)// authentication check
            {
                string currentUserId = Context.User.Identity.GetUserId();
                bool saved = false;
                try
                {
                    using (db = new Entities())
                    {
                        var dbKiosk = kioskById(kiosk.Id);//kiosk in db
                        if (currentUser != null && dbKiosk != null)
                        {
                            if (isAdministrator || doesUserOwnKiosk(dbKiosk))//if user is admin or owns the kiosk then proceed
                            {
                                dbKiosk.Address = kiosk.Address;
                                dbKiosk.Email = kiosk.Email;
                                dbKiosk.Phone = kiosk.Phone;
                                dbKiosk.StoreContact = kiosk.StoreContact;
                                dbKiosk.KioskDisplayName = kiosk.KioskDisplayName;

                                var location = dbKiosk.KioskLocations.FirstOrDefault(a => a.EndDate == null);//as location is subtable we getting current one (one without end date)
                                if (location == null)
                                {
                                    if (!String.IsNullOrWhiteSpace(kiosk.LocationName))//location is null, and user typed someting in the field 
                                    {
                                        db.KioskLocations.Add(new KioskLocation() //adding new (first) location
                                        {
                                            StartDate = DateTime.Now,
                                            Location = kiosk.LocationName,
                                            Kiosk_Id = dbKiosk.Id
                                        });
                                    }
                                }
                                else if (kiosk.LocationName != location.Location)//location already exists, and user typed new one
                                {
                                    var now = DateTime.Now;
                                    location.EndDate = now;//closing old location time period
                                    db.KioskLocations.Add(new KioskLocation() //adding new location
                                    {
                                        StartDate = now + TimeSpan.FromTicks(1),//just to prevent time overlap between locations
                                        Location = kiosk.LocationName,
                                        Kiosk_Id = dbKiosk.Id
                                    });
                                }

                                if (isAdministrator)//only administrator can change ownership
                                {
                                    var ownership = db.KioskOwnerships.FirstOrDefault(a => a.Kiosk_Id == kiosk.Id && a.OwnershipEnd == null);//check who user owns the kiosk

                                    if (ownership == null && kiosk.OwnerId != null) //not owned kiosk is assigned
                                    {
                                        var newOwner = db.AspNetUsers.FirstOrDefault(a => a.Id == kiosk.OwnerId);
                                        if (newOwner != null)
                                            db.KioskOwnerships.Add(new KioskOwnership() { AspNetUsers_Id = newOwner.Id, Kiosk_Id = dbKiosk.Id, OwnershipStart = DateTime.Now });
                                    }
                                    else if (ownership != null && kiosk.OwnerId == null)//owned kiosk assigned to no one
                                    {
                                        ownership.OwnershipEnd = DateTime.Now;
                                    }
                                    else if (ownership != null && kiosk.OwnerId != ownership.AspNetUsers_Id)//owned kiosk assigned to different user
                                    {
                                        var newOwner = db.AspNetUsers.FirstOrDefault(a => a.Id == kiosk.OwnerId);
                                        if (newOwner != null)
                                        {
                                            var now = DateTime.Now;
                                            ownership.OwnershipEnd = now; //ending existing ownership perdiod
                                            db.KioskOwnerships.Add(new KioskOwnership() { AspNetUsers_Id = newOwner.Id, Kiosk_Id = dbKiosk.Id, OwnershipStart = now + TimeSpan.FromTicks(1) });
                                        }
                                    }
                                }
                                db.SaveChanges();
                                saved = true;
                                kiosk = new Models.KioskVM(dbKiosk);//updating kioskVM with values from db , to make sure they are same , as this object will be send back to clients 
                            }
                        }
                    }
                }
                catch (Exception e)
                {

                }

                if (saved)//if ok let everyone with access to kiosk know 
                {
                    BroadcastKioskUpdated(kiosk);
                }
                else//if failed let orginator know and give him current db state of the kiosk
                {
                    try
                    {
                        using (db = new Entities())
                        {
                            kiosk = new Models.KioskVM(db.Kiosks.FirstOrDefault(x => x.Id == kiosk.Id));//refreshing kiosk state from db
                        }
                    }
                    catch (Exception e)
                    {

                    }
                    kiosk.UpdateFailed = true; //letting client know update failed
                    Clients.Caller.kioskUpdateFailed(kiosk);
                }

            }
        }

    }
}