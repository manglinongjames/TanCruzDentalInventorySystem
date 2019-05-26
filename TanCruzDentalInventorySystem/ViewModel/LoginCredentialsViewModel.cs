using System.ComponentModel.DataAnnotations;

namespace TanCruzDentalInventorySystem.ViewModel
{
    public class LoginCredentialsViewModel
    {
        [Required]
        [StringLength(20)]
        public string UserName { get; set; }

        [Required]
        [StringLength(200)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }


    }
}