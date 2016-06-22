using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KioskMessageTypes
{
    [DataContract]
    public class TransactionMessageWrap
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public Nullable<System.DateTime> Started { get; set; }

        [DataMember]
        public Nullable<System.DateTime> Ended { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Gender { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Postcode { get; set; }

        [DataMember]
        public string AddressLine1 { get; set; }

        [DataMember]
        public string AddressLine2 { get; set; }

        [DataMember]
        public string AddressLine3 { get; set; }

        [DataMember]
        public Nullable<int> TotalScore { get; set; }

        [DataMember]
        public Nullable<int> Test1Score { get; set; }

        [DataMember]
        public Nullable<int> Test2Score { get; set; }

        [DataMember]
        public Nullable<int> Test3Score { get; set; }

        [DataMember]
        public Nullable<bool> isSuccessful { get; set; }

        [DataMember]
        public Nullable<bool> isSent { get; set; }

        [DataMember]
        public string Age { get; set; }

        [DataMember]
        public string Test1Answer1 { get; set; }

        [DataMember]
        public string Test1Answer2 { get; set; }

        [DataMember]
        public string Test1Answer3 { get; set; }

        [DataMember]
        public string Test1Answer4 { get; set; }

        [DataMember]
        public string Test1Answer5 { get; set; }

        [DataMember]
        public string Test2Answer1 { get; set; }

        [DataMember]
        public string Test2Answer2 { get; set; }

        [DataMember]
        public string Test2Answer3 { get; set; }

        [DataMember]
        public string Test2Answer4 { get; set; }

        [DataMember]
        public string Test2Answer5 { get; set; }

        [DataMember]
        public string Test2Answer6 { get; set; }

        [DataMember]
        public string Test2Answer7 { get; set; }

        [DataMember]
        public string Test2Answer8 { get; set; }

        [DataMember]
        public string Test3Answer1 { get; set; }

        [DataMember]
        public string Test3Answer2 { get; set; }

        [DataMember]
        public string Test3Answer3 { get; set; }

        [DataMember]
        public string Test3Answer4 { get; set; }

        [DataMember]
        public string Test3Answer5 { get; set; }

        [DataMember]
        public string KioskId { get; set; }
        
        [DataMember]
        public decimal timeTaken { get; set; }


    }
}
