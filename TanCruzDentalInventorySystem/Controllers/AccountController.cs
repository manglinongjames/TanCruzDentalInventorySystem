using AutoMapper;
using IdentityManagement.Entities;
using IdentityManagement.Mvc;
using IdentityManagement.Utilities;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TanCruzDentalInventorySystem.BusinessService.BusinessServiceInterface;
using TanCruzDentalInventorySystem.ViewModel;

namespace TanCruzDentalInventorySystem.Controllers
{
	[Authorize(Roles = "Administrator")]
	public class AccountController : BaseIdentityController
	{
		private readonly IAccountService _accountService;

		public AccountController(IAccountService accountService)
		{
			_accountService = accountService;
		}

		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl ?? "/";
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Login(LoginViewModel loginInfo, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				ApplicationUser oUser = await SignInManager.UserManager.FindAsync(loginInfo.UserName, loginInfo.Password);

				if (oUser != null)
				{
					switch (oUser.UserStatus)
					{
						case EnumUserStatus.Pending:
							ModelState.AddModelError(string.Empty, "Error: User account has not been verified.");
							break;
						case EnumUserStatus.Active:
							await SignInManager.SignInAsync(oUser, loginInfo.RememberMe, false);
							return Redirect(returnUrl ?? "/");

						case EnumUserStatus.Banned:
							ModelState.AddModelError(string.Empty, "Error: User account has been banned.");
							break;
						case EnumUserStatus.LockedOut:
							ModelState.AddModelError(string.Empty, "Error: User account has been locked out due to multiple login tries.");
							break;
					}
				}
				else
					ModelState.AddModelError(string.Empty, "Invalid login details.");
			}
			return View(loginInfo);

			//try
			//{
			//    var userProfile = _accountService.Login(loginInfo);
			//}
			//catch
			//{

			//}
			// Instead of creating the view, redirect to another controller on login success/fail.
			//return Redirect(returnUrl);
		}

		[AllowAnonymous]
		public ActionResult Logout()
		{
			SignInManager.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
			return RedirectToAction("Login", "Account");
		}

		public ActionResult UserList()
		{
			var users = Mapper.Map<List<UserViewModel>>(UserManager.Users);
			return View(users);
		}

		public ActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Register(RegisterViewModel registerViewModel)
		{
			if (ModelState.IsValid)
			{
				var user = new ApplicationUser()
				{
					UserName = registerViewModel.UserName,
					FirstName = registerViewModel.FirstName,
					MiddleName = registerViewModel.MiddleName,
					LastName = registerViewModel.LastName,
					Email = registerViewModel.Email,
					UserStatus = registerViewModel.UserStatus
				};


				var result = await UserManager.CreateAsync(user, registerViewModel.Password);
				if (result.Succeeded)
					return RedirectToAction("UserList");
				else
					foreach (var error in result.Errors)
						ModelState.AddModelError(string.Empty, error);
			}

			return View(registerViewModel);
		}

		public async Task<ActionResult> UserGroups(string userId)
		{
			var user = await UserManager.FindByIdAsync(userId);
			var appGroups = GroupManager.Groups;
			var userGroups = GroupManager.GetUserGroups(userId);

			var mapped = appGroups.Where(group => userGroups.Any(userGroup => userGroup.GroupId == group.GroupId))
				.Select(g => new SelectGroupViewModel()
				{
					GroupId = g.GroupId,
					GroupName = g.GroupName,
					GroupDescription = g.GroupDescription,
					IsSelected = true
				});

			var notMapped = appGroups.Where(group => !userGroups.Any(userGroup => userGroup.GroupId == group.GroupId))
				.Select(g => new SelectGroupViewModel()
				{
					GroupId = g.GroupId,
					GroupName = g.GroupName,
					GroupDescription = g.GroupDescription,
					IsSelected = false
				});

			var allGroups = new List<SelectGroupViewModel>();
			allGroups.AddRange(mapped);
			allGroups.AddRange(notMapped);

			var selectGroup = new SelectUserGroupsViewModel()
			{
				UserId = user.UserId,
				UserName = user.UserName,
				LastName = user.LastName,
				FirstName = user.FirstName,
				Groups = allGroups.OrderBy(g => g.GroupName).ToList()
			};

			//foreach (var group in appGroups)
			//{
			//	var mappedGroup = new SelectGroupViewModel()
			//	{
			//		GroupId = group.GroupId,
			//		GroupName = group.GroupName,
			//		GroupDescription = group.GroupDescription,
			//		IsSelected = userGroups.Exists(g => g.GroupId == group.GroupId)
			//	};
			//	selectGroup.Groups.Add(mappedGroup);
			//}
			return View(selectGroup);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> UserGroups(SelectUserGroupsViewModel selGroups)
		{
			if (ModelState.IsValid)
			{
				var userGroups = GroupManager.GetUserGroups(selGroups.UserId);

				var deletedGroupsIds = selGroups.Groups.Where(group => userGroups.Any(userGroup => userGroup.GroupId == group.GroupId && !group.IsSelected))
					.Select(group => group.GroupId);
				var newGroupsIds = selGroups.Groups.Where(group => !userGroups.Any(userGroup => userGroup.GroupId == group.GroupId)).Where(group => group.IsSelected)
					.Select(group => group.GroupId);

				await GroupManager.RemoveUserFromGroupAsync(selGroups.UserId, deletedGroupsIds);
				await GroupManager.AddUserToGroup(selGroups.UserId, newGroupsIds);

				return RedirectToAction("UserList");
			}
			return View();
		}

		public async Task<ActionResult> UserPermissions(string userId)
		{
			var user = await UserManager.FindByIdAsync(userId);
			var userRoles = await UserManager.GetRolesAsync(userId);
			var appRoles = RoleManager.Roles;

			var userPermissions = new UserPermissionsViewModel()
			{
				UserId = user.UserId,
				UserName = user.UserName,
				Roles = appRoles.Where(role => userRoles.Any(userRole => userRole == role.Name))
					.Select(r => new RoleViewModel
					{
						RoleId = r.RoleId,
						RoleName = r.RoleName,
						RoleDescription = r.RoleDescription
					}).OrderBy(r => r.RoleName)
			};

			return View(userPermissions);
		}

		public ActionResult GroupList()
		{
			var appGroups = GroupManager.Groups;

			var groupViewModel = appGroups
				.Select(group => new GroupViewModel
				{
					GroupId = group.GroupId,
					GroupName = group.GroupName,
					GroupDescription = group.GroupDescription
				}).OrderBy(g => g.GroupName);

			return View(groupViewModel);
		}

		public ActionResult RoleList()
		{
			var appRoles = RoleManager.Roles;

			var roleViewModel = appRoles
				.Select(role => new RoleViewModel
				{
					RoleId = role.RoleId,
					RoleName = role.RoleName,
					RoleDescription = role.RoleDescription
				}).OrderBy(r => r.RoleName);

			return View(roleViewModel);
		}

		public async Task<ActionResult> GroupRoles(string groupId)
		{
			var appRoles = RoleManager.Roles;
			var groupRoles = await GroupManager.GetGroupRoles(groupId);
			var group = await GroupManager.FindByIdAsync(groupId);


			var mapped = appRoles.Where(role => groupRoles.Any(groupRole => groupRole.RoleId == role.RoleId))
				.Select(r => new SelectRoleViewModel()
				{
					RoleId = r.RoleId,
					RoleName = r.RoleName,
					RoleDescription = r.RoleDescription,
					IsSelected = true
				});

			var notMapped = appRoles.Where(role => !groupRoles.Any(groupRole => groupRole.RoleId == role.RoleId))
				.Select(r => new SelectRoleViewModel()
				{
					RoleId = r.RoleId,
					RoleName = r.RoleName,
					RoleDescription = r.RoleDescription,
					IsSelected = false
				});

			var allRoles = new List<SelectRoleViewModel>();
			allRoles.AddRange(mapped);
			allRoles.AddRange(notMapped);

			var selectRole = new SelectGroupRolesViewModel()
			{
				GroupId = group.GroupId,
				GroupName = group.GroupName,
				GroupDescription = group.GroupDescription,
				Roles = allRoles.OrderBy(r => r.RoleName).ToList()
			};

			return View(selectRole);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> GroupRoles(SelectGroupRolesViewModel selRoles)
		{
			if (ModelState.IsValid)
			{
				var groupRoles = await GroupManager.GetGroupRoles(selRoles.GroupId);

				var deletedRolesIds = selRoles.Roles.Where(role => groupRoles.Any(groupRole => groupRole.RoleId == role.RoleId && !role.IsSelected))
					.Select(role => role.RoleId);
				var newRolesIds = selRoles.Roles.Where(role => !groupRoles.Any(groupRole => groupRole.RoleId == role.RoleId)).Where(role => role.IsSelected)
					.Select(role => role.RoleId);

				await GroupManager.RemoveRoleFromGroupAsync(selRoles.GroupId, deletedRolesIds);
				await GroupManager.AddRoleToGroup(selRoles.GroupId, newRolesIds);

				return RedirectToAction("GroupList");
			}
			return View();
		}

		public async Task<ActionResult> EditUser(string userId)
		{
			var user = await UserManager.FindByIdAsync(userId);
			var editUser = new EditUserViewModel()
			{
				UserId = user.UserId,
				UserName = user.UserName,
				LastName = user.LastName,
				FirstName = user.FirstName,
				MiddleName = user.MiddleName,
				Email = user.Email,
				UserStatus = user.UserStatus
			};
			return View(editUser);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> EditUser(EditUserViewModel userModel)
		{
			if (ModelState.IsValid)
			{
				var updatedUser = await UserManager.FindByIdAsync(userModel.UserId);
				updatedUser.UserName = userModel.UserName;
				updatedUser.LastName = userModel.LastName;
				updatedUser.FirstName = userModel.FirstName;
				updatedUser.MiddleName = userModel.MiddleName;
				updatedUser.Email = userModel.Email;
				updatedUser.UserStatus = userModel.UserStatus;

				var result = await UserManager.UpdateAsync(updatedUser);
				if (result.Succeeded)
					return RedirectToAction("UserList");
				else
					foreach (var error in result.Errors)
						ModelState.AddModelError(string.Empty, error);
			}

			// If we got this far, something failed, redisplay form
			return View(userModel);
		}

		[AllowAnonymous]
		public ActionResult ChangePassword(string message)
		{
			if (!User.Identity.IsAuthenticated)
				return RedirectToAction("Login");

			ViewBag.StatusMessage = message;

			return View();
			//ViewBag.StatusMessage =
			//	message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
			//	: message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
			//	: message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
			//	: message == ManageMessageId.Error ? "An error has occurred."
			//	: "";
			//ViewBag.HasLocalPassword = HasPassword();
			//ViewBag.ReturnUrl = Url.Action("Manage");
			//return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			if (!User.Identity.IsAuthenticated)
				return RedirectToAction("Login");

			ViewBag.ReturnUrl = Url.Action("ChangePassword");
			
			if (ModelState.IsValid)
			{
				IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
				if (result.Succeeded)
					return RedirectToAction("ChangePassword", new { message = "Your password has been changed." });
				else
					foreach (var error in result.Errors)
						ModelState.AddModelError("", error);
			}
			
			// If we got this far, something failed, redisplay form
			return View(model);
		}
	}
}