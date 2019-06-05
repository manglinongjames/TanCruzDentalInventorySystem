using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;

namespace IdentityManagement.Mvc
{
    public class BaseIdentityController : Controller
    {
        protected ApplicationSignInManager _signInManager;
        protected ApplicationUserManager _userManager;
        protected ApplicationRoleManager _roleManager;
        protected ApplicationGroupManager _groupManager;

        protected BaseIdentityController() { }


        protected ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        protected ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        protected ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        protected ApplicationGroupManager GroupManager
        {
            get
            {
                return _groupManager ?? HttpContext.GetOwinContext().Get<ApplicationGroupManager>();
            }
            private set
            {
                _groupManager = value;
            }
        }
    }
}

