using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TanCruzDentalInventorySystem.Models
{
    public class UserProfile
    {
        public string UserId { get; set; }
        public UserGroup UserGroup { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string RowStatus { get; set; }
    }
}