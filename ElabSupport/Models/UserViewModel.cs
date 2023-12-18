using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElabSupport.Models
{
    public class UserViewModel
    {
        public UserModel user { get; set; }
        public List<UserModel> UserList { get; set; }
        public List<UseRoleModel> UserRoles { get; set; }
    }

}