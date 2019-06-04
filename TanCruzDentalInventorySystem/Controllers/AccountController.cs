using IdentityManagement.Entities;
using IdentityManagement.Mvc;
using IdentityManagement.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using TanCruzDentalInventorySystem.BusinessService.BusinessServiceInterface;
using TanCruzDentalInventorySystem.ViewModel;

namespace TanCruzDentalInventorySystem.Controllers
{
    [Authorize]
    public class AccountController : BaseIdentityController
    {
        private readonly IAccountService _accountService;

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

                if (oUser != null && new PasswordHasher().VerifyHashedPassword(oUser.Password, loginInfo.Password) == PasswordVerificationResult.Success)
                {
                    if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                        SignInManager.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

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
                    ModelState.AddModelError(string.Empty, "Invalid login details.");
                }
            }
            return View(loginInfo);

            //try
            //{
            //    var userProfile = _accountService.Login(loginInfo);
            //}
            //catch
            //{

            //}
            // Instead of creating the view, redirect to another controller on login success/fail.
            //return Redirect(returnUrl);
        }

        public ActionResult Logout()
        {
            SignInManager.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }
    }
}