using KioskMessageTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HearingSolutionWebApp.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IKioskDataService" in both code and config file together.
    // Added 15/06/2016
    [ServiceContract]
    public interface IKioskDataService
    {
        [OperationContract]
        bool SendTransaction(TransactionMessageWrap transaction);

        [OperationContract]
        bool SendHeartbeatMessage(HeartbeatMessageWrap heartbeat);


    }
}
