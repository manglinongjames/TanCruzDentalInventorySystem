using AutoMapper;
using IdentityManagement.Entities;
using TanCruzDentalInventorySystem.Models;
using TanCruzDentalInventorySystem.ViewModel;

namespace TanCruzDentalInventorySystem
{
    public static class AutoMapperConfig
    {
        public static void RegisterComponents()
        {
            Mapper.Initialize(cfg =>
            {
                //cfg.CreateMap<UserProfile, UserProfileViewModel>().ReverseMap();
                cfg.CreateMap<ApplicationUser, EditUserViewModel>().ReverseMap();
            });
        }
    }
}