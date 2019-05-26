using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace TanCruzDentalInventorySystem.Models
{
    public class UserGroup
    {
        public string UserGroupId { get; set; }
        public string UserGroupName { get; set; }
        public string UserGroupDescription { get; set; }
        public string RowStatus { get; set; }
    }
}