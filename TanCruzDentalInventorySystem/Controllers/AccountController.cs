using IdentityManagement.Entities;
using IdentityManagement.Mvc;
using IdentityManagement.Utilities;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using TanCruzDentalInventorySystem.ViewModel;

namespace TanCruzDentalInventorySystem.Controllers
{
	[Authorize(Roles = "Administrator")]
	public class AccountController : BaseIdentityController
	{
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

		#region Role Management
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
		#endregion

		#region User Management
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
				var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
				if (result.Succeeded)
					return RedirectToAction("ChangePassword", new { message = "Your password has been changed." });
				else
					foreach (var error in result.Errors)
						ModelState.AddModelError("", error);
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		public async Task<ActionResult> EditUser(string userId)
		{
			if (string.IsNullOrWhiteSpace(userId))
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			var user = await UserManager.FindByIdAsync(userId);
			var editUser = new UserViewModel()
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
		public async Task<ActionResult> EditUser(UserViewModel userModel)
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
				var newUser = new ApplicationUser()
				{
					UserName = registerViewModel.UserName,
					FirstName = registerViewModel.FirstName,
					MiddleName = registerViewModel.MiddleName,
					LastName = registerViewModel.LastName,
					Email = registerViewModel.Email,
					UserStatus = registerViewModel.UserStatus
				};


				var result = await UserManager.CreateAsync(newUser, registerViewModel.Password);
				if (result.Succeeded)
					return RedirectToAction("UserList");
				else
					foreach (var error in result.Errors)
						ModelState.AddModelError(string.Empty, error);
			}

			return View(registerViewModel);
		}

		public ActionResult UserList()
		{
			var usersViewModel = new List<UserViewModel>();

			foreach (ApplicationUser user in UserManager.Users)
			{
				usersViewModel.Add(new UserViewModel()
				{
					UserId = user.UserId,
					UserName = user.UserName,
					LastName = user.LastName,
					FirstName = user.FirstName,
					MiddleName = user.MiddleName,
					Email = user.Email,
					UserStatus = user.UserStatus
				});
			}
			return View(usersViewModel);
		}

		public async Task<ActionResult> UserPermissions(string userId)
		{
			if (string.IsNullOrWhiteSpace(userId))
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

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
		#endregion

		#region Group Management
		public ActionResult CreateGroup()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> CreateGroup(GroupViewModel groupViewModel)
		{
			if (ModelState.IsValid)
			{
				var newGroup = new ApplicationGroup()
				{
					GroupName = groupViewModel.GroupName,
					GroupDescription = groupViewModel.GroupDescription
				};

				var result = await GroupManager.CreateAsync(newGroup);
				if (result.Succeeded)
					return RedirectToAction("GroupList");
				else
					foreach (var error in result.Errors)
						ModelState.AddModelError("", error);

			}

			return View(groupViewModel);
		}

		public async Task<ActionResult> DeleteGroup(string groupId)
		{
			if (string.IsNullOrWhiteSpace(groupId))
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			var group = await GroupManager.FindByIdAsync(groupId);
			var editGroup = new GroupViewModel()
			{
				GroupId = group.GroupId,
				GroupName = group.GroupName,
				GroupDescription = group.GroupDescription
			};

			return View(editGroup);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteGroupConfirmed(string groupId)
		{
			var deleteGroup = await GroupManager.FindByIdAsync(groupId);
			await GroupManager.DeleteAsync(deleteGroup);
			return RedirectToAction("GroupList");
		}

		public async Task<ActionResult> EditGroup(string groupId)
		{
			if (string.IsNullOrWhiteSpace(groupId))
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			var group = await GroupManager.FindByIdAsync(groupId);
			var editGroup = new GroupViewModel()
			{
				GroupId = group.GroupId,
				GroupName = group.GroupName,
				GroupDescription = group.GroupDescription
			};

			return View(editGroup);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> EditGroup(GroupViewModel groupViewModel)
		{
			if (ModelState.IsValid)
			{
				var updatedGroup = await GroupManager.FindByIdAsync(groupViewModel.GroupId);
				updatedGroup.GroupName = groupViewModel.GroupName;
				updatedGroup.GroupDescription = groupViewModel.GroupDescription;

				var result = await GroupManager.UpdateAsync(updatedGroup);
				if (result.Succeeded)
					return RedirectToAction("GroupList");
				else
					foreach (var error in result.Errors)
						ModelState.AddModelError(string.Empty, error);
			}

			// If we got this far, something failed, redisplay form
			return View(groupViewModel);
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

		public async Task<ActionResult> GroupRoles(string groupId)
		{
			if (string.IsNullOrWhiteSpace(groupId))
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

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

				await GroupManager.RemoveRolesFromGroupAsync(selRoles.GroupId, deletedRolesIds);
				await GroupManager.AddRoleToGroup(selRoles.GroupId, newRolesIds);

				return RedirectToAction("GroupList");
			}
			return View();
		}

		public async Task<ActionResult> UserGroups(string userId)
		{
			if (string.IsNullOrWhiteSpace(userId))
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

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
		#endregion
	}
}