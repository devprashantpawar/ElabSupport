using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElabSupport.Models
{
    public class LoginModel
    {
    public string UserName { get; set; }
    public string Password { get; set; }
    }
    public class UpdateStatusRequest
    {
        public string UserId { get; set; }
        public bool Status { get; set; }
        public string DeviceId {get;set;}
    }

}