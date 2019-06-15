namespace TanCruzDentalInventorySystem.ViewModel
{
	public class ItemViewModel
	{
		public string ItemId { get; set; }
		public string ItemName { get; set; }
		public string ItemDescription { get; set; }
		public ItemGroupViewModel ItemGroup { get; set; }
	}

	public class ItemGroupViewModel
	{
		public string ItemGroupId { get; set; }
		public string ItemGroupName { get; set; }
		public string ItemGroupDescription { get; set; }
	}

	public class ItemPriceViewModel
	{
		public string ItemPriceId { get; set; }
		public ItemViewModel Item { get; set; }
		public string ItemPriceName { get; set; }
		public string ItemPriceDescription { get; set; }
		public string Type { get; set; }
		public decimal PriceAmount { get; set; }
		public string BaseCurrency { get; set; }
	}
}