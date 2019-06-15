using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TanCruzDentalInventorySystem.BusinessService.BusinessServiceInterface;
using TanCruzDentalInventorySystem.Repository.DataServiceInterface;
using TanCruzDentalInventorySystem.ViewModel;

namespace TanCruzDentalInventorySystem.BusinessService
{
	public class ItemService : IItemService
	{
		private readonly IItemRepository _itemRepository;

		public ItemService(IUnitOfWork unitOfWork, IItemRepository itemRepository)
		{
			_itemRepository = itemRepository;
			_itemRepository.UnitOfWork = unitOfWork;
		}

		public async Task<ItemViewModel> GetItem(string itemId)
		{
			var item = await _itemRepository.GetItem(itemId);
			return Mapper.Map<ItemViewModel>(item);
		}

		public async Task<IEnumerable<ItemViewModel>> GetItemList()
		{
			var itemList = await _itemRepository.GetItemList();
			return Mapper.Map<List<ItemViewModel>>(itemList);
		}


		//public UserProfileViewModel Login(LoginViewModel loginInfo)
		//{
		//    _accountRepository.UnitOfWork = _unitOfWork;
		//    // null;

		//    return Mapper.Map<UserProfileViewModel>(_accountRepository.Login(loginInfo.UserName, loginInfo.Password));

		//    //_unitOfWork.Begin();
		//    //try
		//    //{
		//    //    _accountRepository.Login(loginInfo.UserName, loginInfo.Password);

		//    //    _unitOfWork.Commit();
		//    //}
		//    //catch (Exception ex)
		//    //{
		//    //    _unitOfWork.Rollback();
		//    //    throw;
		//    //}

		//    //return false;
		//}
	}
}