using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElabSupport.Models
{
    public class UseRoleModel
    {
        public int UserRoleId { get; set; }
        public string UserRole { get; set; }
        public decimal? Rates { get; set; }
        public string UserRoleDescription { get; set; }
        public int? Shift1 { get; set; }
        public int? Shiftboth { get; set; }
    }
}