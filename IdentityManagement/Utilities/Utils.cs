using System;
using System.Web;

namespace IdentityManagement.Utilities
{
    public class Utils
    {
        internal static String ConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString;
        }

        internal static bool IfUserAuthenticated()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return true;
            }
            return false;
        }

        internal static bool IfUserInRole(string roleName)
        {
            if (IfUserAuthenticated())
            {
                if (HttpContext.Current.User.IsInRole(roleName))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
