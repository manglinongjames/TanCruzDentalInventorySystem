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

		public ActionResult Logout()
		{
			SignInManager.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
			return RedirectToAction("Login", "Account");
		}

		public ActionResult ListUsers()
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
					UserStatus = EnumUserStatus.Active
				};


				var result = await UserManager.CreateAsync(user, registerViewModel.Password);
				if (result.Succeeded)
					return RedirectToAction("ListUsers", "Account");
				else
					foreach (var error in result.Errors)
						ModelState.AddModelError(string.Empty, error);
			}

			return View(registerViewModel);
		}

		public async Task<ActionResult> UserGroups(string userId)
		{
			var user = await UserManager.FindByIdAsync(userId);
			var appGroups = GroupManager.Groups.ToList();
			var userGroups = GroupManager.GetUserGroups(userId).ToList();

			var selectGroup = new SelectUserGroupsViewModel()
			{
				UserId = user.UserId,
				UserName = user.UserName,
				LastName = user.LastName,
				FirstName = user.FirstName,
				Groups = new List<SelectGroupViewModel>()
			};

			foreach (var group in appGroups)
			{
				var mappedGroup = new SelectGroupViewModel()
				{
					GroupId = group.GroupId,
					GroupName = group.GroupName,
					GroupDescription = group.GroupDescription,
					IsSelected = userGroups.Exists(g => g.GroupId == group.GroupId)
				};
				selectGroup.Groups.Add(mappedGroup);
			}
			//UserManager.
			//IList<string> Groups = await UserManager.GetGroupsAsync(user.UserId);
			return View(selectGroup);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> UserGroups(SelectUserGroupsViewModel selGroups)
		{
			if (ModelState.IsValid)
			{
				var userGroups = GroupManager.GetUserGroups(selGroups.UserId).ToList();

				var deletedGroupsIds = selGroups.Groups.Where(group => userGroups.Any(userGroup => userGroup.GroupId == group.GroupId && !group.IsSelected))
					.Select(group => group.GroupId);
				var newGroupsIds = selGroups.Groups.Where(group => !userGroups.Any(userGroup => userGroup.GroupId == group.GroupId)).Where(group => group.IsSelected)
					.Select(group => group.GroupId);

				await GroupManager.RemoveUserFromGroupAsync(selGroups.UserId, deletedGroupsIds);
				await GroupManager.AddUserToGroup(selGroups.UserId, newGroupsIds);

				return RedirectToAction("ListUsers");
			}
			return View();
		}
	}
}