using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElabSupport.Models
{
    public class UserModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DeviceId { get; set; }
        public int UserRoleId { get; set; }
        public int RateType { get; set; }
        public decimal? Rates { get; set; }
        public int Active { get; set; }
        public string EmailId { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        
    }

}