using IdentityManagement.Data;
using IdentityManagement.Entities;
using System.Collections.Generic;

namespace IdentityManagement.Repositories
{
    public class GroupRepository
    {
        public static IList<ApplicationGroup> GetGroups()
        {
            IList<ApplicationGroup> oGroupList = SqlHelper.GetRecords<ApplicationGroup>("GetGroups", null);
            return oGroupList;
        }
    }
}
