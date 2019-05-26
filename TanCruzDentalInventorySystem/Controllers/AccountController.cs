using System.Web.Mvc;
using TanCruzDentalInventorySystem.BusinessService.BusinessServiceInterface;
using TanCruzDentalInventorySystem.ViewModel;

namespace TanCruzDentalInventorySystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginCredentialsViewModel loginInfo)
        {
            UserProfileViewModel userProfile = null;

            try
            {
                userProfile = _accountService.Login(loginInfo);
            }
            catch
            {
                
            }
            // Instead of creating the view, redirect to another controller on login success/fail.
            return RedirectToAction("Index", "Home");
        }
    }
}