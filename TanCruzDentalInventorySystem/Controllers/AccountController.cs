using System;
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

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginCredentialsViewModel loginInfo)
        {
            // TODO: return the correct view for successful/failed login
            bool successful = false;
            try
            {
                successful = _accountService.Login(loginInfo);

            }
            catch (Exception ex)
            {
                successful = false;
            }
            // Instead of creating the view, redirect to another controller on login success/fail.
            return Content($"Login result: {successful}");
        }
    }
}