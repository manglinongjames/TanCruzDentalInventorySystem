using IdentityManagement.Utilities;
using System.Collections.Generic;
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

	public class UserViewModel
	{
		public string UserId { get; set; }
		public string UserName { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public EnumUserStatus UserStatus { get; set; }
	}

	public class RegisterViewModel
	{
		[Required]
		[Display(Name = "User name")]
		public string UserName { get; set; }

		[Required]
		[StringLength(100, ErrorMessage =
			"The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage =
			"The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		[Required]
		[Display(Name = "First Name")]
		public string FirstName { get; set; }

		[Required]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }

		[Display(Name = "Middle Name")]
		public string MiddleName { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "Select a correct User Status")]
		public EnumUserStatus UserStatus { get; set; }
	}

	public class EditUserViewModel
	{
		public string UserId { get; set; }

		[Required]
		[Display(Name = "User name")]
		public string UserName { get; set; }

		[Required]
		[Display(Name = "First Name")]
		public string FirstName { get; set; }

		[Required]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }

		[Display(Name = "Middle Name")]
		public string MiddleName { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "Select a correct User Status")]
		public EnumUserStatus UserStatus { get; set; }
	}

	public class SelectUserGroupsViewModel
	{
		public string UserId { get; set; }
		public string UserName { get; set; }
		public string LastName { get; set; }
		public string FirstName { get; set; }
		public List<SelectGroupViewModel> Groups { get; set; }
	}

	public class SelectGroupRolesViewModel
	{
		public string GroupId { get; set; }
		public string GroupName { get; set; }
		public string GroupDescription { get; set; }
		public List<SelectRoleViewModel> Roles { get; set; }
	}

	public class GroupViewModel
	{
		public string GroupId { get; set; }
		public string GroupName { get; set; }
		public string GroupDescription { get; set; }
	}

	public class SelectGroupViewModel : GroupViewModel
	{
		public bool IsSelected { get; set; } = false;
	}

	public class UserPermissionsViewModel
	{
		public string UserId { get; set; }
		public string UserName { get; set; }
		public IEnumerable<RoleViewModel> Roles { get; set; }
	}

	public class SelectRoleViewModel : RoleViewModel
	{
		public bool IsSelected { get; set; } = false;
	}

	public class RoleViewModel
	{
		public string RoleId { get; set; }
		public string RoleName { get; set; }
		public string RoleDescription { get; set; }
	}
}