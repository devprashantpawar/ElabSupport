using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElabSupport.Models
{
    public class AccountSupportData
    {
        public string OOSPerson { get; set; }
        public string UserRole { get; set; }
        public string Days { get; set; }
        public string TotalAmount { get; set; }
    }
    public class UserAccountData
    {
        public string ScheduleDate { get; set; }
        public string OOSPerson { get; set; }
        public string UserRole { get; set; }
        public string Day { get; set; }
        public string Amount { get; set; }
        public string MobileNumber { get; set; }
    }
    public class ActiveLogs
    {
        public string UserId { get; set; }
        public string Datetime { get; set; }
        public bool Flag { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        
    }
}