using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using HearingSolutionWebApp.Hubs;

namespace HearingSolutionWebApp.Models
{
    public class KioskVM
    {
        public KioskVM() { }


        public KioskVM(Kiosk dbKiosk)
        {
            Id = dbKiosk.Id;
            KioskEvokeId = dbKiosk.KioskID;
            KioskDisplayName = dbKiosk.KioskDisplayName;
            StoreContact = dbKiosk.StoreContact;
            Email = dbKiosk.Email;
            Phone = dbKiosk.Phone;
            LocationName = dbKiosk.KioskLocations.FirstOrDefault(a => a.EndDate == null)?.Location;
            CompanyName = dbKiosk.KioskOwnerships.FirstOrDefault(a => a.OwnershipEnd == null)?.AspNetUser.CompanyName;
            OwnerId= dbKiosk.KioskOwnerships.FirstOrDefault(a => a.OwnershipEnd == null)?.AspNetUser.Id;
            DateDeleted = dbKiosk.DateDeleted;
            KioskType = dbKiosk.KioskType;
        }


        public Guid Id { get; set; }

        public string KioskEvokeId { get; set; }

        public string KioskDisplayName { get; set; }

        public string CompanyName { get; set; }

        public string LocationName { get; set; }

        public string Address { get; set; }

        public string StoreContact { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public Nullable<System.DateTime> DateDeleted { get; set; }

        public string KioskType { get; set; }

        public bool UpdateFailed { get; set; }

        public bool IsNew { get; set; }

        public string OwnerId { get; set; }
    }
}