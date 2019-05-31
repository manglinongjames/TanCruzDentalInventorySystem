using System.Web.Mvc;

namespace TanCruzDentalInventorySystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }


        public ActionResult Login()
        {
            ViewBag.Title = "Welcome to Tan Cruz Dental Inventory System";
            return View();
        }
    }
}
