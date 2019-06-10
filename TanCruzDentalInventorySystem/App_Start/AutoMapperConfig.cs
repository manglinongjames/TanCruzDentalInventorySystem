using AutoMapper;
using IdentityManagement.Entities;
using TanCruzDentalInventorySystem.ViewModel;

namespace TanCruzDentalInventorySystem
{
	public static class AutoMapperConfig
	{
		public static void RegisterComponents()
		{
			Mapper.Initialize(cfg =>
			{
				//cfg.CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
				//cfg.CreateMap<ApplicationUser, RegisterViewModel>().ReverseMap();
				//cfg.CreateMap<ApplicationRole, RoleViewModel>().ReverseMap();
			});
		}
	}
}