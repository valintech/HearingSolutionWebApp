using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HearingSolutionWebApp.Hubs
{
    public class ConnectedUser
    {
        public string ConnectionId { get; set; }
        public string UserInDBId { get; set; }
    }
}