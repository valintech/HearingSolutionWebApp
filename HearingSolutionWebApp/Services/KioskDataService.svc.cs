using HearingSolutionWebApp.Models;
using KioskMessageTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HearingSolutionWebApp.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "KioskDataService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select KioskDataService.svc or KioskDataService.svc.cs at the Solution Explorer and start debugging.
    public class KioskDataService : IKioskDataService
    {
        public bool SendTransaction(TransactionMessageWrap transaction)
        {
            try
            {
                using (Entities dbConnection = new Entities())
                {
                    var entriesToCheck = dbConnection.Transactions.Where(a => a.Name != transaction.Name && a.DateTaken != a.DateTaken);
                    if (entriesToCheck.Count() == 0 || entriesToCheck == null)
                    {
                        var kioskOwner = dbConnection.KioskOwnerships.FirstOrDefault(a => a.Kiosk.KioskID == transaction.KioskId && a.OwnershipStart <= transaction.Started && (a.OwnershipEnd >= transaction.Ended || a.OwnershipEnd == null));
                        if (kioskOwner == null)
                        {
                            return false;
                        }
                        else
                        {
                            var newTransaction = new Transaction()
                            {

                                KioskOwnership = kioskOwner,
                                KioskOwnership_Id = kioskOwner.Id,
                                Name = transaction.Name,
                                Phone = transaction.Phone,
                                Email = transaction.Email,
                                Age = transaction.Age,
                                Gender = transaction.Gender,
                                AdressLine1 = transaction.AddressLine1,
                                AdressLine2 = transaction.AddressLine2,
                                AdressLine3 = transaction.AddressLine3,
                                Postcode = transaction.Postcode,
                                TestOneQ1 = transaction.Test1Answer1,
                                TestOneQ2 = transaction.Test1Answer2,
                                TestOneQ3 = transaction.Test1Answer3,
                                TestOneQ4 = transaction.Test1Answer4,
                                TestOneQ5 = transaction.Test1Answer5,
                                TestTwoQ1 = transaction.Test2Answer1,
                                TestTwoQ2 = transaction.Test2Answer2,
                                TestTwoQ3 = transaction.Test2Answer3,
                                TestTwoQ4 = transaction.Test2Answer4,
                                TestTwoQ5 = transaction.Test2Answer5,
                                TestTwoQ6 = transaction.Test2Answer6,
                                TestTwoQ7 = transaction.Test2Answer7,
                                TestTwoQ8 = transaction.Test2Answer8,
                                TestThreeQ1 = transaction.Test3Answer1,
                                TestThreeQ2 = transaction.Test3Answer2,
                                TestThreeQ3 = transaction.Test3Answer3,
                                TestThreeQ4 = transaction.Test3Answer4,
                                TestThreeQ5 = transaction.Test3Answer5,
                                SectionOneScore = transaction.Test1Score,
                                SectionTwoScore = transaction.Test2Score,
                                SectionThreeScore = transaction.Test3Score,
                                TotalScore = transaction.TotalScore,
                                DateTaken = transaction.Ended,
                                IsSuccessful = transaction.isSuccessful,
                                DateUploaded = DateTime.Now,
                                TimeTaken = transaction.timeTaken


                            };
                            dbConnection.Transactions.Add(newTransaction);
                            dbConnection.SaveChanges();

                        }
                    }
                    return true;
                }
            }
            catch (Exception e)
            {
                var i = 0;
                return false;
            }

        }

        public bool SendHeartbeatMessage(HeartbeatMessageWrap heartbeat)
        {

            try
            {
                using (Entities dbConnection = new Entities())
                {
                    var newHeartbeat = new KioskHeartbeat()
                    {
                        Kiosk_Id = dbConnection.Kiosks.FirstOrDefault(a => a.KioskID == heartbeat.KioskId).Id,
                        PaperStatus = heartbeat.PaperStatus,
                        HeartbeatTime = heartbeat.HeartbeatTime,
                        Kiosk = dbConnection.Kiosks.FirstOrDefault(a => a.KioskID == heartbeat.KioskId)
                    };
                    if (newHeartbeat.Kiosk == null || newHeartbeat.Kiosk_Id == null)
                    {
                        return false;
                    }
                    else
                    {
                        dbConnection.KioskHeartbeats.Add(newHeartbeat);
                        dbConnection.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                var i = 0;
                return false;
            }


        }
    }
}
