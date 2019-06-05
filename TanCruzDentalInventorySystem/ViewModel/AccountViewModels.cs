using AutoMapper;
using IdentityManagement.Entities;
using IdentityManagement.Utilities;
using System.ComponentModel.DataAnnotations;

namespace TanCruzDentalInventorySystem.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class EditUserViewModel
    {
        public EditUserViewModel() { }

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public EnumUserStatus UserStatus { get; set; }
    }
}