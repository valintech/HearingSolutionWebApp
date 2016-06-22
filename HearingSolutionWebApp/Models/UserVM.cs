using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HearingSolutionWebApp.Models
{
    public class UserVM
    {
        public UserVM() { }


        public UserVM(AspNetUser dbUser)
        {
            Id = new Guid(dbUser.Id);
            UserName = dbUser.UserName;
            CompanyName = dbUser.CompanyName;
        }


        public Guid Id { get; set; }

        public string Ids { get {return Id.ToString(); } set { Id = new Guid(value); } }

        public string CompanyName { get; set; }

        public string UserName { get; set; }
    }
}