using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KioskMessageTypes
{

    //Added 15/06/2016
    [DataContract]
    public class HeartbeatMessageWrap
    {
        [DataMember]
        public Nullable<System.DateTime> HeartbeatTime { get; set; }

        [DataMember]
        public string KioskId { get; set; }

        [DataMember]
        public string PaperStatus { get; set; }

    }
}
