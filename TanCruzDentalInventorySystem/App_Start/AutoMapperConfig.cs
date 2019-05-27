using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                cfg.CreateMap<UserProfile, UserProfileViewModel>().ReverseMap();
            });
        }
    }
}