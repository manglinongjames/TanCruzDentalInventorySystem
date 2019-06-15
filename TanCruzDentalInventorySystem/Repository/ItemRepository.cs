using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using TanCruzDentalInventorySystem.Models;
using TanCruzDentalInventorySystem.Repository.DataServiceInterface;

namespace TanCruzDentalInventorySystem.Repository
{
	public class ItemRepository : IItemRepository
    {
        public IUnitOfWork UnitOfWork { get; set; }

		public async Task<IEnumerable<Item>> GetItemList()
		{
			// TODO: Retrieve ItemList from datastore.
			var itemList = await UnitOfWork.Connection.QueryAsync<Item, ItemGroup, Item>(
				sql: SP_GET_ITEM_LIST,
				(item, itemGroup) =>
				{
					item.ItemGroup = itemGroup;
					return item;
				},
				param: null,
				transaction: UnitOfWork.Transaction,
				commandType: System.Data.CommandType.StoredProcedure,
				splitOn: "ItemGroupId");

			return itemList;
		}

		public async Task<Item> GetItem(string itemId)
		{
			var parameters = new DynamicParameters();
			parameters.Add("ITEM_ID", itemId, System.Data.DbType.String, System.Data.ParameterDirection.Input);

			var item = await UnitOfWork.Connection.QueryAsync<Item, ItemGroup, Item>(
				sql: SP_GET_ITEM,
				(itemUnit, itemGroup) =>
				{
					itemUnit.ItemGroup = itemGroup;
					return itemUnit;
				},
				param: parameters,
				transaction: UnitOfWork.Transaction,
				commandType: System.Data.CommandType.StoredProcedure,
				splitOn: "ItemGroupId");

			return item.AsList().SingleOrDefault();
		}

		//public UserProfile Login(string userName, string password)
		//      {
		//          var parameters = new DynamicParameters();
		//          parameters.Add("@USER_NAME", userName, System.Data.DbType.String, System.Data.ParameterDirection.Input);

		//          var userProfile = UnitOfWork.Connection.QuerySingleOrDefault<UserProfile>(
		//              sql: GET_USERPROFILE,
		//              param: parameters,
		//              transaction: UnitOfWork.Transaction,
		//              commandType: System.Data.CommandType.Text);

		//          return userProfile;
		//      }


		private const string SP_GET_ITEM_LIST = "dbo.GetItems";
		private const string SP_GET_ITEM = "dbo.GetItem";
	}
}