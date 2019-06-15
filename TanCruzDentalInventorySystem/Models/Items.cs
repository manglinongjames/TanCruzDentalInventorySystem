using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TanCruzDentalInventorySystem.Models
{
	public class Item
	{
		public string ItemId { get; set; }
		public string ItemName { get; set; }
		public string ItemDescription { get; set; }
		public ItemGroup ItemGroup { get; set; }
	}

	public class ItemGroup
	{
		public string ItemGroupId { get; set; }
		public string ItemGroupName { get; set; }
		public string ItemGroupDescription { get; set; }
		public string UserId { get; set; }
	}
}