using IdentityManagement.Entities;
using IdentityManagement.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TanCruzDentalInventorySystem.BusinessService.BusinessServiceInterface;
using TanCruzDentalInventorySystem.ViewModel;

namespace TanCruzDentalInventorySystem.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationSignInManager SignInManager
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

        public ApplicationUserManager UserManager
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

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }



        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl ?? "/";
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginCredentialsViewModel loginInfo, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser oUser = await SignInManager.UserManager.FindByNameAsync(loginInfo.UserName);

                if (new PasswordHasher().VerifyHashedPassword(oUser.Password, loginInfo.Password) == PasswordVerificationResult.Success)
                {
                    switch (oUser.UserStatus)
                    {
                        case EnumUserStatus.Pending:
                            ModelState.AddModelError(string.Empty, "Error: User account has not been verified.");
                            break;
                        case EnumUserStatus.Active:
                            SignInManager.SignIn(oUser, loginInfo.RememberMe, false);
                            return Redirect(returnUrl ?? "/");

                        case EnumUserStatus.Banned:
                            ModelState.AddModelError(string.Empty, "Error: User account has been banned.");
                            break;
                        case EnumUserStatus.LockedOut:
                            ModelState.AddModelError(string.Empty, "Error: User account has been locked out due to multiple login tries.");
                            break;
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Error: Invalid login details.");
                }
            }
            return View(loginInfo);

            //try
            //{
            //    userProfile = _accountService.Login(loginInfo);
            //}
            //catch
            //{

            //}
            //// Instead of creating the view, redirect to another controller on login success/fail.
            //return Redirect(returnUrl);
        }

        public ActionResult Logout()
        {
            SignInManager.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Account");
        }
    }
}