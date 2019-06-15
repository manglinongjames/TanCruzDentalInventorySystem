using System.Threading.Tasks;
using System.Web.Mvc;
using TanCruzDentalInventorySystem.BusinessService.BusinessServiceInterface;

namespace TanCruzDentalInventorySystem.Controllers
{
    public class ItemController : Controller
    {
		private IItemService _itemService;

		public ItemController(IItemService itemService)
		{
			_itemService = itemService;
		}
        // GET: ItemHome

        public async Task<ActionResult> ItemHome()
        {
			var items = await _itemService.GetItemList();

            return View();
        }

        public async Task<ActionResult> ItemRecord(string itemId)
        {
			var item = await _itemService.GetItem(itemId);

            return View();
        }
    }
}